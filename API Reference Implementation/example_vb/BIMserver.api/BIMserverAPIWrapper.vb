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

Imports System.Diagnostics
Imports System.IO
Imports Microsoft.Win32

Namespace BIMserverAPIVB

    Public Enum BIMserverAPILanguage
        SPANISH
        ENGLISH
        FRENCH
        ITALIAN
        DEUSTCH
    End Enum


    Public Module BIMserverAPIWrapper

        Private bsDebugMode As Boolean = False
        Private bsApiExecutableName As String = "bsapicmdln.exe"
        Private bsDataBaseFolder As String = Nothing
        Private bsLoginFolder As String = Nothing
        Private bsLanguage As String = "EN" 'ES, EN, FR, IT, DE

#Region "API Calls"
        Private Const bsAPICall_create_database As String = "create_database"
        Private Const bsAPICall_is_logged As String = "is_logged"
        Private Const bsAPICall_login As String = "login"

        Private Const bsAPICall_get_logged_user_name As String = "get_logged_user_name"
        Private Const bsAPICall_get_logged_user_email As String = "get_logged_user_email"
        Private Const bsAPICall_get_logged_user_image As String = "get_logged_user_image"

        Private Const bsAPICall_connect As String = "connect"
        Private Const bsAPICall_disconnect As String = "disconnect"
        Private Const bsAPICall_send_user_recover_password_email As String = "send_user_recover_password_email"

        Private Const bsAPICall_select_current_project As String = "select_current_project"
        Private Const bsAPICall_get_current_project_path As String = "get_current_project_path"

        Private Const bsAPICall_select_ifc_file_from_current_project As String = "select_ifc_file_from_current_project"
        Private Const bsAPICall_put_file_in_current_project As String = "put_file_in_current_project"

        Private Const bsAPICall_select_associated_file_from_current_project As String = "select_associated_file_from_current_project"
        Private Const bsAPICall_put_associated_file_in_current_project As String = "put_associated_file_in_current_project"

        Private Const bsAPICall_get_file_path_in_current_project As String = "get_file_path_in_current_project"
        Private Const bsAPICall_exists_updated_file_version_current_project As String = "exists_updated_file_version_current_project"

        Private Const bsAPIResponse_TRUE As String = "YES"
        Private Const bsAPIResponse_FALSE As String = "NO"
        Private Const bsAPIResponse_ERROR As String = "ERROR"
        Private Const bsAPIResponse_BLOCKED_RESOURCE As String = "BLOCKED_RESOURCE"
#End Region

#Region "API Configuration"

        Public Sub setDebugMode(ByVal debugMode As Boolean)
            bsDebugMode = debugMode
        End Sub
        Private Function getApiExecutablePath() As String
            Dim ApiExecutablePath As String

            Dim localMachineKey64bits As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Dim key As RegistryKey = localMachineKey64bits.OpenSubKey("SOFTWARE\BIMserver.center\Synchronizer")

            If (key Is Nothing) Then
                ApiExecutablePath = Nothing
            Else
                ApiExecutablePath = key.GetValue("InstallDir").ToString()
                key.Close()
            End If

            Return ApiExecutablePath
        End Function

        Public Function isApiInstalled() As Boolean
            Dim ApiExecutablePath As String

            ApiExecutablePath = getApiExecutablePath()
            Return (ApiExecutablePath <> Nothing)
        End Function

        Public Sub setLoginPath(ByVal login_path As String)
            bsLoginFolder = login_path
        End Sub

        Public Sub setDatabasePath(ByVal database_path As String)
            bsDataBaseFolder = database_path
        End Sub
        Public Sub setLanguage(ByVal language As BIMserverAPILanguage)

            Select Case language

                Case BIMserverAPILanguage.SPANISH

                    bsLanguage = "ES"

                Case BIMserverAPILanguage.ENGLISH

                    bsLanguage = "EN"

                Case BIMserverAPILanguage.FRENCH

                    bsLanguage = "FR"

                Case BIMserverAPILanguage.ITALIAN

                    bsLanguage = "IT"

                Case BIMserverAPILanguage.DEUSTCH

                    bsLanguage = "DE"

                Case Else

                    bsLanguage = "EN"

            End Select
        End Sub

#End Region

#Region "Database"

        Public Function createDatabase() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_create_database)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_create_database, bsDataBaseFolder)
        End Function

#End Region

#Region "Login"

        Public Function isLogged() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_is_logged)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_is_logged, bsLoginFolder)
        End Function

        Public Function loginForm() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_login)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_login, bsLoginFolder)
        End Function

        Public Function connect(ByVal userName As String, ByVal userPassword As String) As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_connect) + " " + userName + " " + userPassword
            Return executeAPICallAndGetStatus(arguments, bsAPICall_connect, bsLoginFolder)
        End Function

        Public Function disconnect() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_disconnect)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_disconnect, bsLoginFolder)
        End Function

        Public Function sendUserRecoverPassword(ByVal userEmail As String) As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_send_user_recover_password_email) + " " + userEmail
            Return executeAPICallAndGetStatus(arguments, bsAPICall_send_user_recover_password_email, bsLoginFolder)
        End Function

        Public Function getLoggedUserName() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_get_logged_user_name)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_name, bsLoginFolder)
        End Function

        Public Function getLoggedUserEmail() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_get_logged_user_email)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_email, bsLoginFolder)
        End Function

        Public Function getLoggedUserImage() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_get_logged_user_image)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_image, bsLoginFolder)
        End Function

#End Region

#Region "Current project management"

        Public Function selectCurrentProject() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_select_current_project)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_select_current_project, bsDataBaseFolder)
        End Function

        Public Function selectIfcFileFromCurrectProject() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_select_ifc_file_from_current_project)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_select_ifc_file_from_current_project, bsDataBaseFolder)
        End Function

        Public Function selectAssociatedFileFromCurrectProject(ByVal extensionFilter As String) As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_select_associated_file_from_current_project) + " " + getFilterByExtensionParameter(extensionFilter)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_select_associated_file_from_current_project, bsDataBaseFolder)
        End Function

        Public Function putFileInCurrentProject(ByVal absoluteFilePath As String) As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_put_file_in_current_project) + " " + stringEnclosedInQuotes(absoluteFilePath)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_put_file_in_current_project, bsDataBaseFolder)
        End Function

        Public Function putAssociatedFileInCurrentProject(
                            ByVal absoluteFilePath As String,
                            ByVal file_description As String,
                            ByVal app_name As String, ByVal app_version As String, ByVal app_description As String) As BIMserverAPIResponse

            Dim arguments As String = makeAPICall(bsAPICall_put_associated_file_in_current_project) + " " + stringEnclosedInQuotes(absoluteFilePath)

            If (file_description <> Nothing And file_description.Length > 0) Then
                arguments += " -file_desc " + " " + stringEnclosedInQuotes(file_description)
            End If

            If (app_name <> Nothing And app_name.Length > 0) Then
                arguments += " -app_name " + " " + stringEnclosedInQuotes(app_name)
            End If

            If (app_version <> Nothing And app_version.Length > 0) Then
                arguments += " -app_version " + " " + stringEnclosedInQuotes(app_version)
            End If

            If (app_description <> Nothing And app_description.Length > 0) Then
                arguments += " -app_description " + " " + stringEnclosedInQuotes(app_description)
            End If

            Return executeAPICallAndGetStatus(arguments, bsAPICall_put_associated_file_in_current_project, bsDataBaseFolder)

        End Function

        Public Function getAbsoluteCurrentProjectPath() As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_get_current_project_path)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_get_current_project_path, bsDataBaseFolder)
        End Function

        Public Function convertRelativeProjectFilePathToAbsolute(ByVal filePathRelativeToProject As String, ByVal updateDatabaseTimeStamp As Boolean) As BIMserverAPIResponse

            Dim arguments As String = makeAPICall(bsAPICall_get_file_path_in_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject) + " " + getUpdateDatabaseTimeStamp(updateDatabaseTimeStamp)
            Return executeAPICallAndGetResponseText(arguments, bsAPICall_get_file_path_in_current_project, bsDataBaseFolder)

        End Function

        Public Function existsUpdatedFileVersion(ByVal filePathRelativeToProject As String) As BIMserverAPIResponse
            Dim arguments As String = makeAPICall(bsAPICall_exists_updated_file_version_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject)
            Return executeAPICallAndGetStatus(arguments, bsAPICall_exists_updated_file_version_current_project, bsDataBaseFolder)
        End Function

#End Region

#Region "API invocation"

        Private Function executeAPICallAndGetStatus(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String) As BIMserverAPIResponse

            Dim get_another_line_for_true_response As Boolean = False
            Return executeAPICallAndProcessResponse(arguments, apiCall, apiResponseFolder, get_another_line_for_true_response)

        End Function

        Private Function executeAPICallAndGetResponseText(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String) As BIMserverAPIResponse

            Dim get_another_line_for_true_response As Boolean = True
            Return executeAPICallAndProcessResponse(arguments, apiCall, apiResponseFolder, get_another_line_for_true_response)

        End Function

        Private Function executeAPICallAndProcessResponse(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String, ByVal get_another_line_for_true_response As Boolean) As BIMserverAPIResponse

            Dim response As BIMserverAPIResponse

            Try
                Call BIMserverAPIWrapper.executeApiCall(arguments, apiResponseFolder)

                Dim responseFileName As String = apiCallResponseFilePath(apiCall, apiResponseFolder)
                Dim fs As StreamReader = File.OpenText(responseFileName)
                Dim loggin_status As String = fs.ReadLine()
                Dim next_line As String

                Select Case loggin_status

                    Case bsAPIResponse_TRUE

                        If (get_another_line_for_true_response = True) Then
                            next_line = fs.ReadLine()
                            response = BIMserverAPIResponse.makeTrueResponseWithResponseText(next_line)
                        Else
                            response = BIMserverAPIResponse.makeTrueResponse()
                        End If

                    Case bsAPIResponse_FALSE

                        response = BIMserverAPIResponse.makeFalseResponse()

                    Case bsAPIResponse_BLOCKED_RESOURCE

                        next_line = fs.ReadLine()
                        response = BIMserverAPIResponse.makeBlockedResourceResponse(next_line)

                    Case bsAPIResponse_ERROR

                        next_line = fs.ReadLine()
                        response = BIMserverAPIResponse.makeErrorResponse(next_line)

                    Case Else

                        response = BIMserverAPIResponse.makeErrorResponse("Unknown response type")

                End Select

                fs.Close()

            Catch ex As Exception

                response = BIMserverAPIResponse.makeErrorResponse(ex.Message)

            End Try

            Return response

        End Function

        Private Sub executeApiCall(ByVal api_parameters As String, ByVal apiResponseFolder As String)
            Dim ApiExecutablePath As String = getApiExecutablePath()

            Try
                Dim process As Process = New Process()
                Dim started As Boolean

                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardOutput = False
                process.StartInfo.RedirectStandardError = False
                process.StartInfo.CreateNoWindow = True
                process.StartInfo.FileName = ApiExecutablePath + "\" + bsApiExecutableName

                process.StartInfo.Arguments = getLanguageParameter()
                process.StartInfo.Arguments += " " + getWorkingDirectoryParameter(apiResponseFolder)
                process.StartInfo.Arguments += " " + api_parameters
                process.StartInfo.Arguments += " " + getDebugParameter()

                started = process.Start()
                Debug.Assert(started = True)

                process.WaitForExit()
            Catch ex As Exception
                MessageBox.Show("Can't find executable file " + bsApiExecutableName + " at '" + ApiExecutablePath + "'")
            End Try
        End Sub

        Private Function stringEnclosedInQuotes(ByVal text As String) As String
            Return """" + text + """"
        End Function

        Private Function getWorkingDirectoryParameter(ByVal apiResponseFolder As String) As String
            Return "-wd " + stringEnclosedInQuotes(apiResponseFolder)
        End Function

        Private Function getLanguageParameter() As String
            Return "-lang " + stringEnclosedInQuotes(bsLanguage)
        End Function

        Private Function getFilterByExtensionParameter(ByVal extensionFilter As String) As String
            Return "-filter_by_file_extension " + stringEnclosedInQuotes(extensionFilter)
        End Function

        Private Function getDebugParameter() As String
            If bsDebugMode = True Then
                Return "-debug"
            Else
                Return ""
            End If
        End Function

        Private Function makeAPICall(ByVal api_call As String) As String
            Return "-" + api_call
        End Function

        Private Function getUpdateDatabaseTimeStamp(ByVal updateDatabaseTimeStamp As Boolean) As String
            If (updateDatabaseTimeStamp = True) Then
                Return ""
            Else
                Return "-no_update_db_timestamp"
            End If
        End Function

        Private Function apiCallResponseFilePath(ByVal api_call As String, ByVal apiResponseFolder As String) As String
            Return apiResponseFolder + "\" + api_call + ".txt"
        End Function
#End Region

    End Module

End Namespace