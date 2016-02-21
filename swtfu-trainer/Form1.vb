Public Class Form1
    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Private WithEvents mouseMoveTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Dim ctrlHeld As Boolean

    Private Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Int32) As IntPtr
    Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByVal lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean
    Private Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
    Private Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As Integer, ByVal lpThreadAttributes As Integer, ByVal dwStackSize As Integer, ByVal lpStartAddress As Integer, ByVal lpParameter As Integer, ByVal dwCreationFlags As Integer, ByRef lpThreadId As Integer) As Integer

    Public Const PROCESS_VM_READ = &H10
    Public Const TH32CS_SNAPPROCESS = &H2
    Public Const MEM_COMMIT = 4096
    Public Const PAGE_READWRITE = 4
    Public Const PROCESS_CREATE_THREAD = (&H2)
    Public Const PROCESS_VM_OPERATION = (&H8)
    Public Const PROCESS_VM_WRITE = (&H20)
    Public Const PROCESS_ALL_ACCESS = &H1F0FFF

    Private _targetProcess As Process = Nothing 'to keep track of it. not used yet.
    Private _targetProcessHandle As IntPtr = IntPtr.Zero 'Used for ReadProcessMemory

    Dim charposptr As Integer

    Dim ctrlOn As Boolean


    Dim mouseStartXPos As Integer
    Dim mouseStartYPos As Integer
    Dim charStartXPos As Single
    Dim charstartYPos As Single
    Dim charstartZpos As Single

    Dim playerXpos As Single
    Dim playerYpos As Single
    Dim playerZpos As Single
    Public Function TryAttachToProcess(ByVal windowCaption As String) As Boolean
        Dim _allProcesses() As Process = Process.GetProcesses
        For Each pp As Process In _allProcesses
            If pp.MainWindowTitle.ToLower.Contains(windowCaption.ToLower) Then
                'found it! proceed.
                Return TryAttachToProcess(pp)
            End If
        Next
        MessageBox.Show("Unable to find process '" & windowCaption & ".' Is running?")
        Return False
    End Function
    Public Function TryAttachToProcess(ByVal proc As Process) As Boolean
        If _targetProcessHandle = IntPtr.Zero Then 'not already attached
            _targetProcess = proc
            _targetProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, _targetProcess.Id)
            If _targetProcessHandle = 0 Then
                TryAttachToProcess = False
                MessageBox.Show("OpenProcess() FAIL! Are you Administrator??")
            Else
                'if we get here, all connected and ready to use ReadProcessMemory()
                TryAttachToProcess = True
                'MessageBox.Show("OpenProcess() OK")
            End If
        Else
            MessageBox.Show("Already attached! (Please Detach first?)")
            TryAttachToProcess = False
        End If
    End Function
    Public Sub DetachFromProcess()
        If Not (_targetProcessHandle = IntPtr.Zero) Then
            _targetProcess = Nothing
            Try
                CloseHandle(_targetProcessHandle)
                _targetProcessHandle = IntPtr.Zero
                MessageBox.Show("MemReader::Detach() OK")
            Catch ex As Exception
                MessageBox.Show("MemoryManager::DetachFromProcess::CloseHandle error " & Environment.NewLine & ex.Message)
            End Try
        End If
    End Sub

    Public Function ReadInt16(ByVal addr As IntPtr) As Int16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToInt16(_rtnBytes, 0)
    End Function
    Public Function ReadInt32(ByVal addr As IntPtr) As Int32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)

        Return BitConverter.ToInt32(_rtnBytes, 0)
    End Function
    Public Function ReadInt64(ByVal addr As IntPtr) As Int64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToInt64(_rtnBytes, 0)
    End Function
    Public Function ReadUInt16(ByVal addr As IntPtr) As UInt16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToUInt16(_rtnBytes, 0)
    End Function
    Public Function ReadUInt32(ByVal addr As IntPtr) As UInt32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToUInt32(_rtnBytes, 0)
    End Function
    Public Function ReadUInt64(ByVal addr As IntPtr) As UInt64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToUInt64(_rtnBytes, 0)
    End Function
    Public Function ReadFloat(ByVal addr As IntPtr) As Single
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToSingle(_rtnBytes, 0)
    End Function
    Public Function ReadDouble(ByVal addr As IntPtr) As Double
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToDouble(_rtnBytes, 0)
    End Function
    Public Function ReadIntPtr(ByVal addr As IntPtr) As IntPtr
        Dim _rtnBytes(IntPtr.Size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, IntPtr.Size, Nothing)
        If IntPtr.Size = 4 Then
            Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
        Else
            Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
        End If
    End Function
    Public Function ReadBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
        Dim _rtnBytes(size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, size, vbNull)
        Return _rtnBytes
    End Function

    Public Sub WriteInt32(ByVal addr As IntPtr, val As Int32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteUInt32(ByVal addr As IntPtr, val As UInt32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteFloat(ByVal addr As IntPtr, val As Single)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteBytes(ByVal addr As IntPtr, val As Byte())
        WriteProcessMemory(_targetProcessHandle, addr, val, val.Length, Nothing)
    End Sub

    Private Shared Sub MouseMoveTimer_Tick() Handles mouseMoveTimer.Tick
        Dim ctrlkey As Boolean
        Dim wKey As Boolean
        Dim sKey As Boolean
        Dim aKey As Boolean
        Dim dKey As Boolean
        Dim spaceKey As Boolean

        wKey = GetAsyncKeyState(Keys.W)
        sKey = GetAsyncKeyState(Keys.S)
        aKey = GetAsyncKeyState(Keys.A)
        dKey = GetAsyncKeyState(Keys.D)
        spaceKey = GetAsyncKeyState(Keys.Space)

        ctrlkey = GetAsyncKeyState(Keys.ControlKey)

        If (ctrlkey) And Not Form1.ctrlHeld Then

            Form1.ctrlHeld = True
            Form1.mouseStartXPos = MousePosition.X
            Form1.mouseStartYPos = MousePosition.Y
            Form1.charStartXPos = Form1.playerXpos
            Form1.charstartYPos = Form1.playerYpos
            Form1.charstartZpos = Form1.playerZpos
            Form1.ctrlOn = Not Form1.ctrlOn
        End If

        If Form1.ctrlOn Then
            Form1.WriteFloat(Form1.charposptr + &HD0, Form1.playerXpos + (MousePosition.X - Form1.mouseStartXPos) * 0.1)
            Form1.WriteFloat(Form1.charposptr + &HD4, Form1.charstartYPos)
            Form1.WriteFloat(Form1.charposptr + &HD8, Form1.playerZpos + (MousePosition.Y - Form1.mouseStartYPos) * 0.1)

            Dim hpPtr As Integer
            hpPtr = Form1.ReadInt32(Form1.ReadInt32(Form1.ReadInt32(&H00D52D54) + &H278) + &H40)
            Form1.WriteFloat(hpPtr + &HF4, Form1.ReadInt32(hpPtr + &HF8))


            Cursor.Position = New Point(Form1.mouseStartXPos, Form1.mouseStartYPos)
        End If

        If wKey Then
            Form1.WriteFloat(Form1.charposptr + &HD0, Form1.playerXpos - 0.3)
        End If
        If sKey Then
            Form1.WriteFloat(Form1.charposptr + &HD0, Form1.playerXpos + 0.3)
        End If

        If aKey Then
            Form1.WriteFloat(Form1.charposptr + &HD8, Form1.playerZpos + 0.3)
        End If
        If dKey Then
            Form1.WriteFloat(Form1.charposptr + &HD8, Form1.playerZpos - 0.3)
        End If
        If spaceKey Then
            'Form1.WriteFloat(Form1.charposptr + &HD4, Form1.playerYpos + 0.3)
            Form1.charstartYPos += 0.3
        End If



        If Not (ctrlkey) Then
            Form1.ctrlHeld = False
        End If
    End Sub
    Private Sub refTimer_Tick() Handles refTimer.Tick
        charposptr = ReadInt32(ReadInt32(ReadInt32(ReadInt32(&H1309400) + &H1D8) + &HE4) + &H20)

        playerXpos = Math.Round(ReadFloat(charposptr + &HD0), 1)
        playerYpos = Math.Round(ReadFloat(charposptr + &HD4), 1)
        playerZpos = Math.Round(ReadFloat(charposptr + &HD8), 1)

        If Not txtXpos.Focused Then txtXpos.Text = playerXpos
        If Not txtYpos.Focused Then txtYpos.Text = playerYpos
        If Not txtZpos.Focused Then txtZpos.Text = playerZpos
    End Sub

    Private Sub txtXpos_TextChanged(sender As Object, e As EventArgs) Handles txtXpos.LostFocus
        WriteFloat(charposptr + &HD0, txtXpos.Text)
    End Sub
    Private Sub txtYpos_TextChanged(sender As Object, e As EventArgs) Handles txtYpos.LostFocus
        WriteFloat(charposptr + &HD4, txtYpos.Text)
    End Sub
    Private Sub txtZpos_TextChanged(sender As Object, e As EventArgs) Handles txtZpos.LostFocus
        WriteFloat(charposptr + &HD8, txtZpos.Text)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refTimer = New System.Windows.Forms.Timer
        refTimer.Interval = 10
        refTimer.Enabled = True
        refTimer.Start()

        TryAttachToProcess("Star Wars: The Force Unleashed")
    End Sub

    Private Sub chkMouseMove_CheckedChanged(sender As Object, e As EventArgs) Handles chkMouseMove.CheckedChanged
        If chkMouseMove.Checked Then
            mouseMoveTimer.Enabled = True
            mouseMoveTimer.Interval = 10
            mouseMoveTimer.Start()
        Else
            mouseMoveTimer.Stop()
        End If
    End Sub
End Class
