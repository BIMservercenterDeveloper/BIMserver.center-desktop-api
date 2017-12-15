<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class example_vb
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
        Me.btnCreateDatabase = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnCreateDatabase
        '
        Me.btnCreateDatabase.Location = New System.Drawing.Point(12, 12)
        Me.btnCreateDatabase.Name = "btnCreateDatabase"
        Me.btnCreateDatabase.Size = New System.Drawing.Size(260, 36)
        Me.btnCreateDatabase.TabIndex = 0
        Me.btnCreateDatabase.Text = "Create database"
        Me.btnCreateDatabase.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(13, 55)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(260, 36)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Login"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 97)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(260, 36)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Check login status"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(13, 139)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(260, 36)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Select current project"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(12, 181)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(260, 36)
        Me.Button4.TabIndex = 4
        Me.Button4.Text = "Select IFC file from current project"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(12, 223)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(260, 36)
        Me.Button5.TabIndex = 5
        Me.Button5.Text = "Select associated file from current project"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(13, 265)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(260, 36)
        Me.Button6.TabIndex = 6
        Me.Button6.Text = "Absolute file paths"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(12, 307)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(260, 36)
        Me.Button7.TabIndex = 7
        Me.Button7.Text = "Put IFC file into current project"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(13, 349)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(260, 36)
        Me.Button8.TabIndex = 8
        Me.Button8.Text = "Put associated IFC file into current project"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(13, 391)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(260, 36)
        Me.Button9.TabIndex = 9
        Me.Button9.Text = "Show current project path"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(13, 433)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(260, 36)
        Me.Button10.TabIndex = 10
        Me.Button10.Text = "Check updated file"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(13, 475)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(260, 36)
        Me.Button11.TabIndex = 11
        Me.Button11.Text = "Show login data"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'example_vb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 522)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnCreateDatabase)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "example_vb"
        Me.Text = "Test API Methods - Visual Basic"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCreateDatabase As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button

End Class
