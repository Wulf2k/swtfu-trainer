<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtXpos = New System.Windows.Forms.TextBox()
        Me.txtZpos = New System.Windows.Forms.TextBox()
        Me.chkMouseMove = New System.Windows.Forms.CheckBox()
        Me.lblZpos = New System.Windows.Forms.Label()
        Me.lblXpos = New System.Windows.Forms.Label()
        Me.txtYpos = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtXpos
        '
        Me.txtXpos.Location = New System.Drawing.Point(72, 20)
        Me.txtXpos.Name = "txtXpos"
        Me.txtXpos.Size = New System.Drawing.Size(63, 20)
        Me.txtXpos.TabIndex = 61
        '
        'txtZpos
        '
        Me.txtZpos.Location = New System.Drawing.Point(72, 64)
        Me.txtZpos.Name = "txtZpos"
        Me.txtZpos.Size = New System.Drawing.Size(63, 20)
        Me.txtZpos.TabIndex = 60
        '
        'chkMouseMove
        '
        Me.chkMouseMove.AutoSize = True
        Me.chkMouseMove.BackColor = System.Drawing.Color.LightGray
        Me.chkMouseMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMouseMove.Location = New System.Drawing.Point(14, 313)
        Me.chkMouseMove.Name = "chkMouseMove"
        Me.chkMouseMove.Size = New System.Drawing.Size(121, 20)
        Me.chkMouseMove.TabIndex = 59
        Me.chkMouseMove.Text = "CtrlMouseMove"
        Me.chkMouseMove.UseVisualStyleBackColor = False
        '
        'lblZpos
        '
        Me.lblZpos.AutoSize = True
        Me.lblZpos.BackColor = System.Drawing.Color.LightGray
        Me.lblZpos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZpos.Location = New System.Drawing.Point(18, 65)
        Me.lblZpos.Name = "lblZpos"
        Me.lblZpos.Size = New System.Drawing.Size(48, 16)
        Me.lblZpos.TabIndex = 58
        Me.lblZpos.Text = "Z pos: "
        '
        'lblXpos
        '
        Me.lblXpos.AutoSize = True
        Me.lblXpos.BackColor = System.Drawing.Color.LightGray
        Me.lblXpos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXpos.Location = New System.Drawing.Point(18, 21)
        Me.lblXpos.Name = "lblXpos"
        Me.lblXpos.Size = New System.Drawing.Size(48, 16)
        Me.lblXpos.TabIndex = 57
        Me.lblXpos.Text = "X pos: "
        '
        'txtYpos
        '
        Me.txtYpos.Location = New System.Drawing.Point(72, 42)
        Me.txtYpos.Name = "txtYpos"
        Me.txtYpos.Size = New System.Drawing.Size(63, 20)
        Me.txtYpos.TabIndex = 63
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.LightGray
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 16)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Y pos: "
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 656)
        Me.Controls.Add(Me.txtYpos)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtXpos)
        Me.Controls.Add(Me.txtZpos)
        Me.Controls.Add(Me.chkMouseMove)
        Me.Controls.Add(Me.lblZpos)
        Me.Controls.Add(Me.lblXpos)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtXpos As TextBox
    Friend WithEvents txtZpos As TextBox
    Friend WithEvents chkMouseMove As CheckBox
    Friend WithEvents lblZpos As Label
    Friend WithEvents lblXpos As Label
    Friend WithEvents txtYpos As TextBox
    Friend WithEvents Label1 As Label
End Class
