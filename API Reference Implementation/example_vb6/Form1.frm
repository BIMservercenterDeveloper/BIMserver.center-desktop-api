VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog CDBox 
      Left            =   3600
      Top             =   960
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()

    Dim res As Boolean
    
    CDBox.DialogTitle = "Choose an EXEC-File ..."
    CDBox.CancelError = True
    CDBox.Filter = "EXEC-Files (*.exe)|*.exe|All files (*.*)|*.*"
    CDBox.ShowOpen
    
    res = SuperShell(CDBox.FileName, Left$(CDBox.FileName, Len(CDBox.FileName) - Len(CDBox.FileTitle)), 0, SW_NORMAL, NORMAL_PRIORITY_CLASS)

End Sub
