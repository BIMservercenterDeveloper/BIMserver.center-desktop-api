Attribute VB_Name = "Module1"
Option Explicit

Const INFINITE = &HFFFF
Const STARTF_USESHOWWINDOW = &H1

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

Private Type SECURITY_ATTRIBUTES
    nLength As Long
    lpSecurityDescriptor As Long
    bInheritHandle As Long
End Type

Public Enum enPriority_Class
    NORMAL_PRIORITY_CLASS = &H20
    IDLE_PRIORITY_CLASS = &H40
    HIGH_PRIORITY_CLASS = &H80
End Enum

Private Declare Function CreateProcess Lib "kernel32" Alias "CreateProcessA" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, lpProcessAttributes As SECURITY_ATTRIBUTES, lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, lpEnvironment As Any, ByVal lpCurrentDriectory As String, lpStartupInfo As STARTUPINFO, lpProcessInformation As PROCESS_INFORMATION) As Long
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long

Public Function SuperShell(ByVal App As String, ByVal WorkDir As String, dwMilliseconds As Long, ByVal start_size As enSW, ByVal Priority_Class As enPriority_Class) As Boolean
    Dim res As Integer
    Dim pClass As Long
    Dim sinfo As STARTUPINFO
    Dim pinfo As PROCESS_INFORMATION
    
    Dim sec1 As SECURITY_ATTRIBUTES
    Dim sec2 As SECURITY_ATTRIBUTES
    
    sec1.nLength = Len(sec1)
    sec2.nLength = Len(sec2)
    
    sinfo.cb = Len(sinfo)
    sinfo.dwFlags = STARTF_USESHOWWINDOW
    sinfo.wShowWindow = start_size
    
    pClass = Priority_Class
    res = CreateProcess(vbNullString, App, sec1, sec2, False, pClass, 0&, WorkDir, sinfo, pinfo)
    
    If res = 0 Then
        WaitForSingleObject pinfo.hProcess, dwMilliseconds
        SuperShell = True
    Else
        SuperShell = False
    End If
        
End Function
