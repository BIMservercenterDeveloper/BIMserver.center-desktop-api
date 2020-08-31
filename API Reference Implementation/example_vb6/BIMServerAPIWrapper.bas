Attribute VB_Name = "BIMServerAPIWrapper"
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

Public Enum BIMserverAPILanguage
    SPANISH
    ENGLISH
    FRENCH
    ITALIAN
    DEUSTCH
End Enum

'API configuration variables
Private bsDebugMode As Boolean
Private bsDataBaseFolder As String
Private bsLoginFolder As String
Private bsLanguage As String

'API names
Private Const bsApiExecutableName As String = "bsapicmdln.exe"

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

Private Const bsAPICall_generate_visualization_file_from_ifc As String = "generate_visualization_file_from_ifc"

'Return code values
Private Const bsAPIResponse_TRUE As String = "YES"
Private Const bsAPIResponse_FALSE As String = "NO"
Private Const bsAPIResponse_ERROR As String = "ERROR"
Private Const bsAPIResponse_BLOCKED_RESOURCE As String = "BLOCKED_RESOURCE"

'Win32 CreateProcessEx
Public Const INFINITE = &HFFFF
Private Const STARTF_USESHOWWINDOW = &H1

Private Const CREATE_DEFAULT_ERROR_MODE = &H4000000
Private Const CREATE_NO_WINDOW = &H8000000
Private Const NORMAL_PRIORITY_CLASS = &H20

Public Enum enSW
    SW_HIDE = 0
    SW_NORMAL = 1
    SW_MAXIMIZE = 3
    SW_MINIMIZE = 6
End Enum

Private Type PROCESS_INFORMATION
        hProcess As Long
        hThread As Long
        dwProcessId As Long
        dwThreadId As Long
End Type

Private Type STARTUPINFO
        cb As Long
        lpReserved As String
        lpDesktop As String
        lpTitle As String
        dwX As Long
        dwY As Long
        dwXSize As Long
        dwYSize As Long
        dwXCountChars As Long
        dwYCountChars As Long
        dwFillAttribute As Long
        dwFlags As Long
        wShowWindow As Integer
        cbReserved2 As Integer
        lpReserved2 As Byte
        hStdInput As Long
        hStdOutput As Long
        hStdError As Long
End Type

Private Declare Function CreateProcessA Lib "kernel32.dll" (ByVal lpApplicationName As Long, ByVal lpCommandLine As _
String, ByVal lpProcessAttributes As Long, ByVal _
lpThreadAttributes As Long, ByVal bInheritHandles As Long, _
ByVal dwCreationFlags As Long, ByVal lpEnvironment As Long, _
ByVal lpCurrentDirectory As Long, lpStartupInfo As _
STARTUPINFO, lpProcessInformation As PROCESS_INFORMATION) As Long

Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long

' Windows API for Register
Const HKEY_LOCAL_MACHINE = &H80000002
Const KEY_QUERY_VALUE = &H1
Const REG_SZ = 1 ' Unicode nul terminated string

Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Long, ByVal lpSubKey As String, ByVal Reserved As Long, ByVal samDesired As Long, phkResult As Long) As Long
Private Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, lpType As Long, lpData As Any, lpcbData As Long) As Long
Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
Public Sub setDebugMode(ByVal debugMode As Boolean)
    bsDebugMode = debugMode
End Sub
'Retrieves the API executable path from the Windows registry
Private Function getApiExecutablePath() As String
    Dim registryKey As Long
    
    RegOpenKeyEx HKEY_LOCAL_MACHINE, "SOFTWARE\BIMserver.center\Synchronizer", 0, KEY_QUERY_VALUE, registryKey
    
    If registryKey = 0 Then
        getApiExecutablePath = vbNullString
    Else
        Dim lResult As Long, lValueType As Long, lDataBufSize As Long
        
        lResult = RegQueryValueEx(registryKey, "InstallDir", 0, lValueType, ByVal 0, lDataBufSize)
        
        If lResult = 0 And lValueType = REG_SZ Then
            Dim stringBuffer As String
            
            stringBuffer = String(lDataBufSize, Chr$(0))
            lResult = RegQueryValueEx(registryKey, "InstallDir", 0, 0, ByVal stringBuffer, lDataBufSize)
            
            If lResult = 0 Then
                getApiExecutablePath = Left$(stringBuffer, InStr(1, stringBuffer, Chr$(0)) - 1)
            Else
                getApiExecutablePath = vbNullString
            End If
        Else
            getApiExecutablePath = vbNullString
        End If
        
        RegCloseKey registryKey
    End If
End Function
Public Function isApiInstalled() As Boolean
    Dim ApiExecutablePath As String
    
    ApiExecutablePath = getApiExecutablePath()
    isApiInstalled = ApiExecutablePath <> vbNullString
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

'Database management
Public Function createDatabase() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_create_database)
    Set createDatabase = executeAPICallAndGetStatus(arguments, bsAPICall_create_database, bsDataBaseFolder)
End Function

'Login management
Public Function isLogged() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_is_logged)
    Set isLogged = executeAPICallAndGetStatus(arguments, bsAPICall_is_logged, bsLoginFolder)
End Function

Public Function loginForm() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_login)
    Set loginForm = executeAPICallAndGetStatus(arguments, bsAPICall_login, bsLoginFolder)
End Function

Public Function connect(ByVal userName As String, ByVal userPassword As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_connect) + " " + userName + " " + userPassword
    Set connect = executeAPICallAndGetStatus(arguments, bsAPICall_connect, bsLoginFolder)
End Function

Public Function disconnect() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_disconnect)
    Set disconnect = executeAPICallAndGetStatus(arguments, bsAPICall_disconnect, bsLoginFolder)
End Function

Public Function sendUserRecoverPassword(ByVal userEmail As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_send_user_recover_password_email) + " " + userEmail
    Set sendUserRecoverPassword = executeAPICallAndGetStatus(arguments, bsAPICall_send_user_recover_password_email, bsLoginFolder)
End Function

Public Function getLoggedUserName() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_get_logged_user_name)
    Set getLoggedUserName = executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_name, bsLoginFolder)
End Function

Public Function getLoggedUserEmail() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_get_logged_user_email)
    Set getLoggedUserEmail = executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_email, bsLoginFolder)
End Function

Public Function getLoggedUserImage() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_get_logged_user_image)
    Set getLoggedUserImage = executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_image, bsLoginFolder)
End Function

'Project management
Public Function selectCurrentProject() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_select_current_project)
    Set selectCurrentProject = executeAPICallAndGetStatus(arguments, bsAPICall_select_current_project, bsDataBaseFolder)
End Function

Public Function selectIfcFileFromCurrectProject() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_select_ifc_file_from_current_project)
    Set selectIfcFileFromCurrectProject = executeAPICallAndGetResponseText(arguments, bsAPICall_select_ifc_file_from_current_project, bsDataBaseFolder)
End Function

Public Function selectAssociatedFileFromCurrectProject(ByVal extensionFilter As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_select_associated_file_from_current_project) + " " + getFilterByExtensionParameter(extensionFilter)
    Set selectAssociatedFileFromCurrectProject = executeAPICallAndGetResponseText(arguments, bsAPICall_select_associated_file_from_current_project, bsDataBaseFolder)
End Function

Public Function putFileInCurrentProject(ByVal absoluteFilePath As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_put_file_in_current_project) + " " + stringEnclosedInQuotes(absoluteFilePath)
    Set putFileInCurrentProject = executeAPICallAndGetStatus(arguments, bsAPICall_put_file_in_current_project, bsDataBaseFolder)
End Function

Public Function putAssociatedFileInCurrentProject(ByVal absoluteFilePath As String, _
                    ByVal file_description As String, _
                    ByVal app_name As String, ByVal app_version As String, ByVal app_description As String) As BIMserverAPIResponse

    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_put_associated_file_in_current_project) & " " & stringEnclosedInQuotes(absoluteFilePath)

    If (file_description <> vbNullString And Len(file_description) > 0) Then
        arguments = arguments & " -file_desc " & " " & stringEnclosedInQuotes(file_description)
    End If

    If (app_name <> vbNullString And Len(app_name) > 0) Then
        arguments = arguments & " -app_name " & " " & stringEnclosedInQuotes(app_name)
    End If

    If (app_version <> vbNullString And Len(app_version) > 0) Then
        arguments = arguments & " -app_version " & " " & stringEnclosedInQuotes(app_version)
    End If

    If (app_description <> vbNullString And Len(app_description) > 0) Then
        arguments = arguments & " -app_description " & " " & stringEnclosedInQuotes(app_description)
    End If

    Set putAssociatedFileInCurrentProject = executeAPICallAndGetStatus(arguments, bsAPICall_put_associated_file_in_current_project, bsDataBaseFolder)

End Function

Public Function getAbsoluteCurrentProjectPath() As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_get_current_project_path)
    Set getAbsoluteCurrentProjectPath = executeAPICallAndGetResponseText(arguments, bsAPICall_get_current_project_path, bsDataBaseFolder)
End Function

Public Function convertRelativeProjectFilePathToAbsolute(ByVal filePathRelativeToProject As String, ByVal updateDatabaseTimeStamp As Boolean) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_get_file_path_in_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject) + " " + getUpdateDatabaseTimeStamp(updateDatabaseTimeStamp)
    Set convertRelativeProjectFilePathToAbsolute = executeAPICallAndGetResponseText(arguments, bsAPICall_get_file_path_in_current_project, bsDataBaseFolder)
End Function

Public Function existsUpdatedFileVersion(ByVal filePathRelativeToProject As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_exists_updated_file_version_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject)
    Set existsUpdatedFileVersion = executeAPICallAndGetStatus(arguments, bsAPICall_exists_updated_file_version_current_project, bsDataBaseFolder)
End Function

Public Function generateVisualizationFileFromIfc(ByVal absoluteInputIFCFilePath As String, ByVal absoluteOutputVisualizationFilePath As String) As BIMserverAPIResponse
    Dim arguments As String
    
    arguments = makeAPICall(bsAPICall_generate_visualization_file_from_ifc) + " " + stringEnclosedInQuotes(absoluteInputIFCFilePath) + " " + stringEnclosedInQuotes(absoluteOutputVisualizationFilePath)
    Set generateVisualizationFileFromIfc = executeAPICallAndGetStatus(arguments, bsAPICall_generate_visualization_file_from_ifc, bsDataBaseFolder)
End Function


' BIMServer API Invocation
Private Function makeTrueResponse() As BIMserverAPIResponse
    Dim response As New BIMserverAPIResponse
    
    response.ResponseType = BIMserverAPIResponseType.vbTrue
    response.responseText = ""
    
    Set makeTrueResponse = response
End Function

Private Function makeTrueResponseWithResponseText(ByVal responseText As String) As BIMserverAPIResponse
    Dim response As New BIMserverAPIResponse
    
    response.ResponseType = BIMserverAPIResponseType.vbTrue
    response.responseText = responseText
    
    Set makeTrueResponseWithResponseText = response
End Function

Private Function makeFalseResponse() As BIMserverAPIResponse
    Dim response As New BIMserverAPIResponse
    
    response.ResponseType = BIMserverAPIResponseType.vbFalse
    response.responseText = ""
    
    Set makeFalseResponse = response
End Function

Private Function makeBlockedResourceResponse(ByVal responseText As String) As BIMserverAPIResponse
    Dim response As New BIMserverAPIResponse
    
    response.ResponseType = BIMserverAPIResponseType.vbBLOCKED_RESOURCE
    response.responseText = responseText
    
    Set makeBlockedResourceResponse = response
End Function

Private Function makeErrorResponse(ByVal responseText As String) As BIMserverAPIResponse
    Dim response As New BIMserverAPIResponse
    
    response.ResponseType = BIMserverAPIResponseType.vbError
    response.responseText = responseText
    
    Set makeErrorResponse = response
End Function
Private Function executeAPICallAndGetStatus(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String) As BIMserverAPIResponse
    Dim get_another_line_for_true_response As Boolean
    
    get_another_line_for_true_response = False
    Set executeAPICallAndGetStatus = executeAPICallAndProcessResponse(arguments, apiCall, apiResponseFolder, get_another_line_for_true_response)
End Function

Private Function executeAPICallAndGetResponseText(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String) As BIMserverAPIResponse
    Dim get_another_line_for_true_response As Boolean
    
    get_another_line_for_true_response = True
    Set executeAPICallAndGetResponseText = executeAPICallAndProcessResponse(arguments, apiCall, apiResponseFolder, get_another_line_for_true_response)
End Function

Private Function executeAPICallAndProcessResponse(ByVal arguments As String, ByVal apiCall As String, ByVal apiResponseFolder As String, ByVal get_another_line_for_true_response As Boolean) As BIMserverAPIResponse
On Error GoTo executeAPICallAndProcessResponseErrorHandler
    Dim response As BIMserverAPIResponse
    Dim errorMessage As String
    Dim res As Boolean
    
    errorMessage = vbNullString
    res = executeAPICAll(arguments, apiResponseFolder, errorMessage)
    
    If res = False Then
        response = makeErrorResponse(errorMessage)
    Else
        Dim responseFileName As String
        
        responseFileName = apiCallResponseFilePath(apiCall, apiResponseFolder)
        
        If LenB(Dir$(responseFileName)) = 0 Then
            response = makeErrorResponse("Couldn't find API response")
        Else
            Dim fileContents As String
            Dim fileLines() As String
            Dim loggin_status, next_line As String
            
            fileContents = getFileContentsUTF16(responseFileName)
            fileLines = Split(fileContents, vbNewLine)
            
            loggin_status = fileLines(0)
            
            Select Case loggin_status
    
                Case bsAPIResponse_TRUE
    
                    If (get_another_line_for_true_response = True) Then
                        next_line = fileLines(1)
                        Set response = makeTrueResponseWithResponseText(next_line)
                    Else
                        Set response = makeTrueResponse()
                    End If
    
                Case bsAPIResponse_FALSE
    
                    Set response = makeFalseResponse()
    
                Case bsAPIResponse_BLOCKED_RESOURCE
    
                    next_line = fileLines(1)
                    Set response = makeBlockedResourceResponse(next_line)
    
                Case bsAPIResponse_ERROR
    
                    next_line = fileLines(1)
                    Set response = makeErrorResponse(next_line)
    
                Case Else
    
                    Set response = makeErrorResponse("Unknown response type")
    
            End Select
        End If
    End If
    
    Set executeAPICallAndProcessResponse = response
    Exit Function
    
executeAPICallAndProcessResponseErrorHandler:
    Set executeAPICallAndProcessResponse = makeErrorResponse("Error reading response file")
    
End Function

Private Function getFileContentsUTF16(fileName As String) As String
    Dim binaryData() As Byte
    Dim fs As Integer
    
    fs = FreeFile
    Open fileName For Binary Access Read As #fs
        ReDim binaryData(FileLen(fileName) - 1)
        Get #fs, , binaryData
    Close #fs
    
    If (binaryData(0) = &HFF And binaryData(1) = &HFE) Or (binaryData(0) = &HFE And binaryData(1) = &HFF) Then
        getFileContentsUTF16 = Mid$(binaryData, 2)
    Else
        getFileContentsUTF16 = binaryData
    End If
    
End Function
        
Private Function executeAPICAll(ByVal apiParameters As String, _
                    ByVal apiResponseFolder As String, _
                    ByRef errorMessage As String) As Boolean
    Dim bsApiExecutablePath As String
    Dim command As String
    Dim parameters As String
    Dim commandLine As String
    Dim pclass As Long
    Dim sinfo As STARTUPINFO
    Dim pinfo As PROCESS_INFORMATION
    Dim res As Integer
    
    bsApiExecutablePath = getApiExecutablePath()
    command = stringEnclosedInQuotes(bsApiExecutablePath & "\" & bsApiExecutableName)
    parameters = getLanguageParameter
    parameters = parameters & " " & getWorkingDirectoryParameter(apiResponseFolder)
    parameters = parameters & " " & apiParameters
    parameters = parameters & " " & getDebugParameter()
    
    commandLine = command & " " & parameters
    
    sinfo.cb = Len(sinfo)
    
    pclass = CREATE_DEFAULT_ERROR_MODE Or NORMAL_PRIORITY_CLASS Or CREATE_NO_WINDOW
    res = CreateProcessA(0&, commandLine, 0&, 0&, False, pclass, 0&, 0&, sinfo, pinfo)
    
    If res = 0 Then
        executeAPICAll = False
        errorMessage = "bsapicmdln.exe not found"
    Else
        res = WaitForSingleObject(pinfo.hProcess, INFINITE)
        
        If res = 0 Then
            executeAPICAll = True
        Else
            executeAPICAll = False
            errorMessage = "Unexpected error while executing api call"
        End If
    End If
End Function

Private Function stringEnclosedInQuotes(ByVal text As String) As String
    stringEnclosedInQuotes = """" & text & """"
End Function

Private Function getWorkingDirectoryParameter(ByVal apiResponseFolder As String) As String
    getWorkingDirectoryParameter = "-wd " & stringEnclosedInQuotes(apiResponseFolder)
End Function

Private Function getLanguageParameter() As String
    getLanguageParameter = "-lang " & stringEnclosedInQuotes(bsLanguage)
End Function

Private Function getFilterByExtensionParameter(ByVal extensionFilter As String) As String
    getFilterByExtensionParameter = "-filter_by_file_extension " & stringEnclosedInQuotes(extensionFilter)
End Function

Private Function getDebugParameter() As String
    If bsDebugMode = True Then
        getDebugParameter = "-debug"
    Else
        getDebugParameter = ""
    End If
End Function

Private Function makeAPICall(ByVal api_call As String) As String
    makeAPICall = "-" & api_call
End Function

Private Function getUpdateDatabaseTimeStamp(ByVal updateDatabaseTimeStamp As Boolean) As String
    If (updateDatabaseTimeStamp = True) Then
        getUpdateDatabaseTimeStamp = ""
    Else
        getUpdateDatabaseTimeStamp = "-no_update_db_timestamp"
    End If
End Function

Private Function apiCallResponseFilePath(ByVal api_call As String, ByVal apiResponseFolder As String) As String
    apiCallResponseFilePath = apiResponseFolder & "\" & api_call & ".txt"
End Function



