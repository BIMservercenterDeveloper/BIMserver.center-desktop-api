VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form Form1 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Test API Methods - Visual Basic"
   ClientHeight    =   7320
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   4770
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7320
   ScaleWidth      =   4770
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog opd 
      Left            =   120
      Top             =   120
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton Command12 
      Caption         =   "Show login data"
      Height          =   500
      Left            =   120
      TabIndex        =   11
      Top             =   6720
      Width           =   4575
   End
   Begin VB.CommandButton Command11 
      Caption         =   "Check updated file"
      Height          =   500
      Left            =   120
      TabIndex        =   10
      Top             =   6120
      Width           =   4575
   End
   Begin VB.CommandButton Command10 
      Caption         =   "Show current project path"
      Height          =   500
      Left            =   120
      TabIndex        =   9
      Top             =   5520
      Width           =   4575
   End
   Begin VB.CommandButton Command9 
      Caption         =   "Put associated IFC file into current project"
      Height          =   500
      Left            =   120
      TabIndex        =   8
      Top             =   4920
      Width           =   4575
   End
   Begin VB.CommandButton Command8 
      Caption         =   "Put IFC file into current project"
      Height          =   500
      Left            =   120
      TabIndex        =   7
      Top             =   4320
      Width           =   4575
   End
   Begin VB.CommandButton Command7 
      Caption         =   "Absolute file paths"
      Height          =   500
      Left            =   120
      TabIndex        =   6
      Top             =   3720
      Width           =   4575
   End
   Begin VB.CommandButton Command6 
      Caption         =   "Select associated file from current project"
      Height          =   500
      Left            =   120
      TabIndex        =   5
      Top             =   3120
      Width           =   4575
   End
   Begin VB.CommandButton Command5 
      Caption         =   "Select IFC file from current project"
      Height          =   500
      Left            =   120
      TabIndex        =   4
      Top             =   2520
      Width           =   4575
   End
   Begin VB.CommandButton Command4 
      Caption         =   "Select current project"
      Height          =   500
      Left            =   120
      TabIndex        =   3
      Top             =   1920
      Width           =   4575
   End
   Begin VB.CommandButton Command3 
      Caption         =   "Check login status"
      Height          =   500
      Left            =   120
      TabIndex        =   2
      Top             =   1320
      Width           =   4575
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Login"
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   4575
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Create Database"
      Height          =   500
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4575
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'BIMserver.center license
'
'This file is part of BIMserver.center IFC frameworks.
'Copyright (c) 2017 BIMserver.center

'Permission is hereby granted, free of charge, to any person obtaining a copy of
'this software and associated documentation files, to use this software with the
'purpose of developing new tools for the BIMserver.center platform or interacting
'with it.
'
'The above copyright notice and this permission notice shall be included in all
'copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
'FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
'COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
'IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
'CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Option Explicit

Dim selectedIfcFileFromCurrentProject As String
Dim selectedAssociatedFileFromCurrentProject As String

'Create database
Private Sub Command1_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.createDatabase
    
    If response.ResponseType = BIMserverAPIResponseType.vbTrue Then
        MsgBox "BIMserverAPI Database created"
    ElseIf response.ResponseType = BIMserverAPIResponseType.vbFalse Then
        MsgBox response.responseText
    Else
        MsgBox response.responseText
    End If
End Sub

Private Sub Command12_Click()
    Dim showLoginForm As New Form2
    
    showLoginForm.populateData
    showLoginForm.Show
    
End Sub

'Show Login Form
Private Sub Command2_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.loginForm()

    Select Case response.ResponseType

        Case BIMserverAPIResponseType.vbTrue

            MsgBox "User is logged"

        Case BIMserverAPIResponseType.vbFalse

            MsgBox "User is not logged"

        Case BIMserverAPIResponseType.vbBLOCKED_RESOURCE

            MsgBox response.responseText

        Case BIMserverAPIResponseType.vbError

            MsgBox response.responseText

    End Select
End Sub
'Check login status
Private Sub Command3_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.isLogged()
    Debug.Assert (response.ResponseType <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

    Select Case response.ResponseType

        Case BIMserverAPIResponseType.vbTrue

            MsgBox "User is logged"

        Case BIMserverAPIResponseType.vbFalse

            MsgBox "User is not logged"

        Case BIMserverAPIResponseType.vbError

            MsgBox response.responseText

    End Select
End Sub
'Select current project
Private Sub Command4_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.selectCurrentProject()

    Select Case response.ResponseType

        Case BIMserverAPIResponseType.vbTrue

            MsgBox response.responseText

        Case BIMserverAPIResponseType.vbFalse

            MsgBox "No project selected or changed"

        Case BIMserverAPIResponseType.vbBLOCKED_RESOURCE

            MsgBox response.responseText

        Case BIMserverAPIResponseType.vbError

            MsgBox response.responseText

    End Select
End Sub
'Select IFC file from current project
Private Sub Command5_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.selectIfcFileFromCurrectProject()
    Debug.Assert (response.ResponseType <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

    Select Case response.ResponseType

        Case BIMserverAPIResponseType.vbTrue

            MsgBox response.responseText
            selectedIfcFileFromCurrentProject = response.responseText

        Case BIMserverAPIResponseType.vbFalse

            MsgBox "No file selected"

        Case BIMserverAPIResponseType.vbError

            MsgBox response.responseText

    End Select
End Sub
' Select associated file from current project
Private Sub Command6_Click()
    Dim response As BIMserverAPIResponse
    
    Set response = BIMServerAPIWrapper.selectAssociatedFileFromCurrectProject("txt;docx;pdf;dwg;dxf")
    Debug.Assert (response.ResponseType <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

    Select Case response.ResponseType

        Case BIMserverAPIResponseType.vbTrue

            MsgBox response.responseText
            selectedAssociatedFileFromCurrentProject = response.responseText

        Case BIMserverAPIResponseType.vbFalse

            MsgBox "No file selected"

        Case BIMserverAPIResponseType.vbError

            MsgBox response.responseText

    End Select

End Sub

Private Function convertRelativeProjectFilePathToAbsolute(ByVal projectRelativeFilePath As String, _
                    ByVal updateDatabaseTimeStamp As Boolean) As String

    If projectRelativeFilePath = "" Then

        convertRelativeProjectFilePathToAbsolute = "No IFC file was selected"

    Else

        Dim response As BIMserverAPIResponse
        
        Set response = BIMServerAPIWrapper.convertRelativeProjectFilePathToAbsolute(projectRelativeFilePath, updateDatabaseTimeStamp)
        Debug.Assert (response.ResponseType = BIMserverAPIResponseType.vbTrue Or response.ResponseType = BIMserverAPIResponseType.vbError)

        convertRelativeProjectFilePathToAbsolute = response.responseText

    End If

End Function
' Show absolute file paths for selected files
Private Sub Command7_Click()
    Dim updateDatabaseTimeStamp As Boolean
    Dim absoluteIfcFilePath, absoluteFilePathAssociatedFile, absolutePaths As String

    updateDatabaseTimeStamp = True
    absoluteIfcFilePath = convertRelativeProjectFilePathToAbsolute(selectedIfcFileFromCurrentProject, updateDatabaseTimeStamp)
    absoluteFilePathAssociatedFile = convertRelativeProjectFilePathToAbsolute(selectedAssociatedFileFromCurrentProject, updateDatabaseTimeStamp)

    absolutePaths = "IFC file: " & absoluteIfcFilePath + vbNewLine
    absolutePaths = absolutePaths & "Associated file: " & absoluteFilePathAssociatedFile

    MsgBox absolutePaths
End Sub


Private Sub Command8_Click()
    opd.Filter = "IFC Files (*.ifc) | *.ifc"
    opd.FilterIndex = 1
    opd.DefaultExt = "ifc"
    opd.DialogTitle = "Select IFC file"
    opd.Flags = cdlOFNPathMustExist Or cdlOFNFileMustExist
    
    opd.ShowOpen
    
    If opd.fileName <> "" Then
        Dim response As BIMserverAPIResponse
        
        Set response = BIMServerAPIWrapper.putFileInCurrentProject(opd.fileName)
        
        If (response.ResponseType = BIMserverAPIResponseType.vbTrue) Then
            MsgBox "File added correctly"
        Else
            Debug.Assert (response.ResponseType = BIMserverAPIResponseType.vbError)
            MsgBox response.responseText
        End If
    End If
End Sub

Private Sub Command9_Click()
    opd.Filter = "Text files (*.txt) | *.txt | PDF files (*.pdf) | *.pdf"
    opd.FilterIndex = 1
    opd.DefaultExt = "ifc"
    opd.DialogTitle = "Select IFC file"
    opd.Flags = cdlOFNPathMustExist Or cdlOFNFileMustExist
    
    opd.ShowOpen
    
    If opd.fileName <> "" Then
        Dim response As BIMserverAPIResponse
        
        Set response = BIMServerAPIWrapper.putAssociatedFileInCurrentProject(opd.fileName, _
                        "An associated file", _
                        "BIMserver.center demo test api", "0.1", "API Demo tools")
        
        If (response.ResponseType = BIMserverAPIResponseType.vbTrue) Then
            MsgBox "File added correctly"
        Else
            Debug.Assert (response.ResponseType = BIMserverAPIResponseType.vbError)
            MsgBox response.responseText
        End If
    End If
End Sub
' Absolute current path
Private Sub Command10_Click()
    Dim response As BIMserverAPIResponse

    Set response = BIMServerAPIWrapper.getAbsoluteCurrentProjectPath()
    
    If (response.ResponseType = BIMserverAPIResponseType.vbTrue) Then
        MsgBox response.responseText
    Else
        Debug.Assert (response.ResponseType = BIMserverAPIResponseType.vbError)
        MsgBox response.responseText
    End If
End Sub
Private Function existsUpdatedVersionOfFile(ByVal relativeFilePathToProject As String) As String

    If (relativeFilePathToProject = vbNullString Or relativeFilePathToProject = "") Then
        existsUpdatedVersionOfFile = "No file selected"
    Else

        Dim response As BIMserverAPIResponse
        
        Set response = BIMServerAPIWrapper.existsUpdatedFileVersion(relativeFilePathToProject)
        Debug.Assert (response.ResponseType <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

        Select Case response.ResponseType

            Case BIMserverAPIResponseType.vbTrue

                existsUpdatedVersionOfFile = "YES"

            Case BIMserverAPIResponseType.vbFalse

                existsUpdatedVersionOfFile = "NO"

            Case Else

                Debug.Assert (response.ResponseType = BIMserverAPIResponseType.vbError)
                existsUpdatedVersionOfFile = response.responseText

        End Select

    End If
End Function
' Check Updated file status
Private Sub Command11_Click()
    Dim updateIfcFileStatus, updateAssociatedFileStatus As String
    Dim status As String

    updateIfcFileStatus = existsUpdatedVersionOfFile(selectedIfcFileFromCurrentProject)
    updateAssociatedFileStatus = existsUpdatedVersionOfFile(selectedAssociatedFileFromCurrentProject)

    status = "IFC file: " & updateIfcFileStatus & vbNewLine
    status = status & "Associated file: " & updateAssociatedFileStatus

    MsgBox status
End Sub
Private Sub Form_Load()

    selectedIfcFileFromCurrentProject = ""
    selectedAssociatedFileFromCurrentProject = ""

    If BIMServerAPIWrapper.isApiInstalled() = False Then
        MsgBox "API not found"
    Else
        'Initialize BIMserver.center API
        BIMServerAPIWrapper.setDebugMode True
        
        BIMServerAPIWrapper.setDatabasePath "c:\ápi aaa"
        BIMServerAPIWrapper.setLoginPath "c:\ápi aaa"
        BIMServerAPIWrapper.setLanguage BIMserverAPILanguage.ENGLISH
    End If
End Sub
