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

Imports System.IO
Imports example_vb.BIMserverAPIVB

Public Class example_vb

    Dim selectedIfcFileFromCurrentProject As String = ""
    Dim selectedAssociatedFileFromCurrentProject As String = ""

    Private Sub example_vb_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        If BIMserverAPIWrapper.isApiInstalled() = False Then
            MsgBox("API not found")
            Application.Exit()
        Else
            BIMserverAPIWrapper.setDebugMode(True)
            BIMserverAPIWrapper.setLanguage(BIMserverAPILanguage.ENGLISH)
            BIMserverAPIWrapper.setDatabasePath("c:\BIMserver.center api demo")
            BIMserverAPIWrapper.setLoginPath("c:\BIMserver.center api demo")

            'This values must be set. You'll get them after registration.
            'BIMserverAPIWrapper.setAppID("app_undefined_id")
            'BIMserverAPIWrapper.setDeveloperID("app_developer_id")
        End If
    End Sub

    Private Sub btnCreateDatabase_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateDatabase.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.createDatabase()

        If (response.type = BIMserverAPIResponseType.vbTRUE) Then
            MsgBox("BIMserverAPI Database created", MsgBoxStyle.OkOnly, "")
        Else
            Debug.Assert(response.type = BIMserverAPIResponseType.vbERROR)
            MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
        End If

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.loginForm()

        Select Case response.type

            Case BIMserverAPIResponseType.vbTRUE

                MsgBox("User is logged", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbFALSE

                MsgBox("User is not logged", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbBLOCKED_RESOURCE

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbERROR

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

        End Select

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.isLogged()
        Debug.Assert(response.type <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

        Select Case response.type

            Case BIMserverAPIResponseType.vbTRUE

                MsgBox("User is logged", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbFALSE

                MsgBox("User is not logged", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbERROR

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

        End Select

    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.selectCurrentProject()

        Select Case response.type

            Case BIMserverAPIResponseType.vbTRUE

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbFALSE

                MsgBox("No project selected or changed", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbBLOCKED_RESOURCE

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbERROR

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

        End Select

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.selectIfcFileFromCurrectProject()
        Debug.Assert(response.type <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

        Select Case response.type

            Case BIMserverAPIResponseType.vbTRUE

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
                selectedIfcFileFromCurrentProject = response.responseText

            Case BIMserverAPIResponseType.vbFALSE

                MsgBox("No file selected", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbERROR

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

        End Select

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.selectAssociatedFileFromCurrectProject("txt;docx;pdf;dwg;dxf")
        Debug.Assert(response.type <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

        Select Case response.type

            Case BIMserverAPIResponseType.vbTRUE

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
                selectedAssociatedFileFromCurrentProject = response.responseText

            Case BIMserverAPIResponseType.vbFALSE

                MsgBox("No file selected", MsgBoxStyle.OkOnly, "")

            Case BIMserverAPIResponseType.vbERROR

                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")

        End Select

    End Sub

    Private Function convertRelativeProjectFilePathToAbsolute(
                        ByVal projectRelativeFilePath As String,
                        ByVal updateDatabaseTimeStamp As Boolean) As String

        If (projectRelativeFilePath = "") Then

            Return "No IFC file was selected"

        Else

            Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.convertRelativeProjectFilePathToAbsolute(projectRelativeFilePath, updateDatabaseTimeStamp)
            Debug.Assert(response.type = BIMserverAPIResponseType.vbTRUE Or response.type = BIMserverAPIResponseType.vbERROR)

            Return response.responseText

        End If

    End Function

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click

        Dim updateDatabaseTimeStamp As Boolean
        Dim absoluteIfcFilePath, absoluteFilePathAssociatedFile, absolutePaths As String

        updateDatabaseTimeStamp = True
        absoluteIfcFilePath = convertRelativeProjectFilePathToAbsolute(selectedIfcFileFromCurrentProject, updateDatabaseTimeStamp)
        absoluteFilePathAssociatedFile = convertRelativeProjectFilePathToAbsolute(selectedAssociatedFileFromCurrentProject, updateDatabaseTimeStamp)

        absolutePaths = "IFC file: " + absoluteIfcFilePath + vbNewLine
        absolutePaths += "Associated file: " + absoluteFilePathAssociatedFile

        MsgBox(absolutePaths, MsgBoxStyle.OkOnly, "")

    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click

        Dim opd As OpenFileDialog = New OpenFileDialog()

        opd.Filter = "IFC Files (*.ifc) | *.ifc"
        opd.FilterIndex = 1
        opd.Multiselect = False

        opd.CheckFileExists = True
        opd.CheckPathExists = True

        Dim selected As DialogResult = opd.ShowDialog()

        If (selected = DialogResult.OK) Then

            Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.putFileInCurrentProject(opd.FileName)

            If (response.type = BIMserverAPIResponseType.vbTRUE) Then
                MsgBox("File added correctly", MsgBoxStyle.OkOnly, "")
            Else
                Debug.Assert(response.type = BIMserverAPIResponseType.vbERROR)
                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
            End If

        End If

    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click

        Dim opd As OpenFileDialog = New OpenFileDialog()

        opd.Filter = "Text files (*.txt) | *.txt | PDF files (*.pdf) | *.pdf"
        opd.FilterIndex = 1
        opd.Multiselect = False

        opd.CheckFileExists = True
        opd.CheckPathExists = True

        Dim selected As DialogResult = opd.ShowDialog()

        If (selected = DialogResult.OK) Then

            Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.putAssociatedFileInCurrentProject(
                        opd.FileName,
                        "An associated file",
                        "BIMserver.center demo test api", "0.1", "API Demo tools")

            If (response.type = BIMserverAPIResponseType.vbTRUE) Then
                MsgBox("File added correctly", MsgBoxStyle.OkOnly, "")
            Else
                Debug.Assert(response.type = BIMserverAPIResponseType.vbERROR)
                MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
            End If

        End If

    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click

        Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.getAbsoluteCurrentProjectPath()

        If (response.type = BIMserverAPIResponseType.vbTRUE) Then
            MsgBox(response.responseText, MsgBoxStyle.OkOnly, "")
        Else
            Debug.Assert(response.type = BIMserverAPIResponseType.vbERROR)
            MessageBox.Show(response.responseText)
        End If

    End Sub

    Private Function existsUpdatedVersionOfFile(ByVal relativeFilePathToProject As String) As String

        If (relativeFilePathToProject = "") Then
            Return "No file selected"
        Else

            Dim response As BIMserverAPIResponse = BIMserverAPIWrapper.existsUpdatedFileVersion(relativeFilePathToProject)
            Debug.Assert(response.type <> BIMserverAPIResponseType.vbBLOCKED_RESOURCE)

            Select Case response.type

                Case BIMserverAPIResponseType.vbTRUE

                    Return "YES"

                Case BIMserverAPIResponseType.vbFALSE

                    Return "NO"

                Case Else

                    Debug.Assert(response.type = BIMserverAPIResponseType.vbERROR)
                    Return response.responseText

            End Select

        End If
    End Function

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Dim updateIfcFileStatus, updateAssociatedFileStatus As String
        Dim status As String

        updateIfcFileStatus = existsUpdatedVersionOfFile(selectedIfcFileFromCurrentProject)
        updateAssociatedFileStatus = existsUpdatedVersionOfFile(selectedAssociatedFileFromCurrentProject)

        status = "IFC file: " + updateIfcFileStatus + vbNewLine
        status += "Associated file: " + updateAssociatedFileStatus

        MsgBox(status, MsgBoxStyle.OkOnly, "")
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        Dim loginDataForm As ShowLoginDataForm = New ShowLoginDataForm()

        loginDataForm.populateData()
        loginDataForm.Show(Me)
    End Sub

End Class
