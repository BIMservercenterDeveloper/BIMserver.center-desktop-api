/*
BIMserver.center license

This file is part of BIMserver.center IFC frameworks.
Copyright (c) 2017 BIMserver.center

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files, to use this software with the
purpose of developing new tools for the BIMserver.center platform or interacting
with it.

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#include "BIMserverAPIWrapper.h"

#include "BIMserverAPIResponse.h"

#include <assert.h>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>

static BOOL bsDebugMode = FALSE;
static const wchar_t *bsApiExecutableName = L"bsapicmdln.exe";

static wchar_t *bsDataBaseFolder = NULL;
static wchar_t *bsLoginFolder = NULL;

static const wchar_t *bsLanguage_SPANISH = L"ES";
static const wchar_t *bsLanguage_ENGLISH = L"EN";
static const wchar_t *bsLanguage_FRENCH = L"FR";
static const wchar_t *bsLanguage_ITALIAN = L"IT";
static const wchar_t *bsLanguage_DEUSTCH = L"DE";
static const wchar_t *bsLanguage = L"EN";

static const wchar_t *bsAPICall_create_database = L"create_database";
static const wchar_t *bsAPICall_is_logged = L"is_logged";
static const wchar_t *bsAPICall_login = L"login";

static const wchar_t *bsAPICall_get_logged_user_name = L"get_logged_user_name";
static const wchar_t *bsAPICall_get_logged_user_email = L"get_logged_user_email";
static const wchar_t *bsAPICall_get_logged_user_image = L"get_logged_user_image";

static const wchar_t *bsAPICall_connect = L"connect";
static const wchar_t *bsAPICall_disconnect = L"disconnect";
static const wchar_t *bsAPICall_send_user_recover_password_email = L"send_user_recover_password_email";

static const wchar_t *bsAPICall_select_current_project = L"select_current_project";
static const wchar_t *bsAPICall_get_current_project_path = L"get_current_project_path";

static const wchar_t *bsAPICall_select_ifc_file_from_current_project = L"select_ifc_file_from_current_project";
static const wchar_t *bsAPICall_put_file_in_current_project = L"put_file_in_current_project";

static const wchar_t *bsAPICall_select_associated_file_from_current_project = L"select_associated_file_from_current_project";
static const wchar_t *bsAPICall_put_associated_file_in_current_project = L"put_associated_file_in_current_project";

static const wchar_t *bsAPICall_get_file_path_in_current_project = L"get_file_path_in_current_project";
static const wchar_t *bsAPICall_exists_updated_file_version_current_project = L"exists_updated_file_version_current_project";

static const wchar_t *bsAPIResponse_TRUE = L"YES";
static const wchar_t *bsAPIResponse_FALSE = L"NO";
static const wchar_t *bsAPIResponse_ERROR = L"ERROR";
static const wchar_t *bsAPIResponse_BLOCKED_RESOURCE = L"BLOCKED_RESOURCE";

#define bsStringBuilderBufferSize 65536

// ----------------------------------------------------------------------------------------------------

static BOOL i_is_api_installed(wchar_t **api_executable_path_opt)
{
    BOOL is_api_installed;
    LONG lRet;
    HKEY hKey;

    lRet = RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\BIMserver.center\\Synchronizer", 0, KEY_QUERY_VALUE, &hKey);

    if (lRet != ERROR_SUCCESS)
    {
        if (api_executable_path_opt != NULL)
            *api_executable_path_opt = NULL;

        is_api_installed = FALSE;
    }
    else
    {
        wchar_t api_executable_path[_MAX_PATH];
        DWORD bufsize;

        api_executable_path[0] = L'\0';
        bufsize = _MAX_PATH;

        lRet = RegGetValueW(hKey, NULL, L"InstallDir", RRF_RT_REG_SZ, NULL, &api_executable_path, &bufsize);

        if (lRet != ERROR_SUCCESS)
        {
            if (api_executable_path_opt != NULL)
                *api_executable_path_opt = NULL;

            is_api_installed = FALSE;
        }
        else
        {
            if (api_executable_path_opt != NULL)
                *api_executable_path_opt = _wcsdup(api_executable_path);

            is_api_installed = TRUE;
        }

        (void)RegCloseKey(hKey);
    }

    return is_api_installed;
}

// ----------------------------------------------------------------------------------------------------

BOOL BIMserverAPIWrapper_is_api_installed(void)
{   
    return i_is_api_installed(NULL);
}

// ----------------------------------------------------------------------------------------------------

void BIMserverAPIWrapper_set_database_path(const wchar_t *database_folder)
{   
    if (bsDataBaseFolder != NULL)
        free(bsDataBaseFolder);

    bsDataBaseFolder = _wcsdup(database_folder);
}

// ----------------------------------------------------------------------------------------------------

void BIMserverAPIWrapper_set_login_path(const wchar_t *login_folder)
{   
    if (bsLoginFolder != NULL)
        free(bsLoginFolder);

    bsLoginFolder = _wcsdup(login_folder);
}

// ----------------------------------------------------------------------------------------------------

void BIMserverAPIWrapper_set_language(BIMserverAPILanguage language)
{
    switch (language)
    {
        case BIMserverAPILanguage_SPANISH:  bsLanguage = bsLanguage_SPANISH; break;
        case BIMserverAPILanguage_ENGLISH:  bsLanguage = bsLanguage_ENGLISH; break;
        case BIMserverAPILanguage_FRENCH:   bsLanguage = bsLanguage_FRENCH;  break;
        case BIMserverAPILanguage_ITALIAN:  bsLanguage = bsLanguage_ITALIAN; break;
        case BIMserverAPILanguage_DEUSTCH:  bsLanguage = bsLanguage_DEUSTCH; break;
        default:                            bsLanguage = bsLanguage_ENGLISH; break;
    }
}

// ----------------------------------------------------------------------------

static BOOL i_format_using_buffer(wchar_t *buffer, size_t buffer_size, const wchar_t *format, va_list argptr)
{
    int result;

    assert(buffer != NULL);
    assert(buffer_size > 0);
    assert(format != NULL);

    result = _vsnwprintf(buffer, buffer_size - 1, format, argptr);
    buffer[buffer_size - 1] = L'\0';

    if (result >= 0)
        return TRUE;
    else
        return FALSE;
}

// ----------------------------------------------------------------------------

static wchar_t *i_make_string_with_format(const WCHAR *format, ...)
{
    wchar_t *buffer_aux;
    va_list argptr;
    wchar_t buffer[bsStringBuilderBufferSize];

    va_start(argptr, format);

    if (i_format_using_buffer(buffer, bsStringBuilderBufferSize, format, argptr) == FALSE)
    {
        unsigned long i;
        wchar_t *buffer2;
        size_t tam_buffer2;

        buffer2 = NULL;
        tam_buffer2 = 0;

        i = 1;

        do
        {
            i++;

            if (buffer2)
            {
                free(buffer2);
                buffer2 = NULL;
            }

            va_end(argptr);
            va_start(argptr, format);

            tam_buffer2 = bsStringBuilderBufferSize * i;
            buffer2 = calloc(tam_buffer2, sizeof(wchar_t));
        }
        while (i_format_using_buffer(buffer2, tam_buffer2, format, argptr) == FALSE);

        buffer_aux = buffer2;
    }
    else
        buffer_aux = _wcsdup(buffer);

    va_end(argptr);

    return buffer_aux;
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_working_directory_parameter(const wchar_t *api_response_folder)
{
    return i_make_string_with_format(L"-wd \"%s\"", api_response_folder);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_language_parameter(void)
{
    return i_make_string_with_format(L"-lang \"%s\"", bsLanguage);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_filter_by_extension_parameter(const wchar_t *extension_filter)
{
    return i_make_string_with_format(L"-filter_by_file_extension \"%s\"", extension_filter);
}

// ----------------------------------------------------------------------------------------------------

static const wchar_t *i_get_debug_parameter(void)
{
    return L"-debug";
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_make_api_call(const wchar_t *api_call)
{
    return i_make_string_with_format(L"-%s", api_call);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_update_database_timestamp_parameter(int updateDatabaseTimeStamp)
{
    if (updateDatabaseTimeStamp == 0)
        return _wcsdup(L"-no_update_db_timestamp");
    else
        return _wcsdup(L"");
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_api_call_response_file_path(const wchar_t *api_call, const wchar_t *api_response_folder)
{
    return i_make_string_with_format(L"%s\\%s.txt", api_response_folder, api_call);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_process_command_string(const wchar_t *api_call_with_arguments, const wchar_t *api_response_folder)
{
    wchar_t *command_string;
    wchar_t *database_parameter, *language_parameter;

    database_parameter = i_get_working_directory_parameter(api_response_folder);
    language_parameter = i_get_language_parameter();

    if (bsDebugMode == TRUE)
        command_string = i_make_string_with_format(L"%s %s %s %s", i_get_debug_parameter(), database_parameter, language_parameter, api_call_with_arguments);
    else
        command_string = i_make_string_with_format(L"%s %s %s", database_parameter, language_parameter, api_call_with_arguments);

    free(database_parameter);
    free(language_parameter);

    return command_string;
}

// ----------------------------------------------------------------------------------------------------

static BOOL i_did_execute_process(const wchar_t *api_call_with_arguments, const wchar_t *api_response_folder, wchar_t **error_message)
{
    BOOL process_executed;
    wchar_t *error_message_aux;
    wchar_t *ApiExecutablePath;

    assert(error_message != NULL);

    if (i_is_api_installed(&ApiExecutablePath) == FALSE)
    {
        process_executed = FALSE;
        error_message_aux = _wcsdup(L"API not found");
    }
    else
    {
        wchar_t *arguments_string, *command;        
        STARTUPINFOW        si;
        PROCESS_INFORMATION procInf;
        unsigned long       creation_flags;
        SECURITY_ATTRIBUTES securityAttributes;
        BOOL res;
        BOOL bInheritHandles;
        DWORD errorlevel;

        arguments_string = i_process_command_string(api_call_with_arguments, api_response_folder);
        command = i_make_string_with_format(L"%s\\%s %s", ApiExecutablePath, bsApiExecutableName, arguments_string);

        creation_flags = CREATE_DEFAULT_ERROR_MODE | NORMAL_PRIORITY_CLASS | CREATE_NO_WINDOW;

        memset(&si, 0, sizeof(si));
        si.cb = sizeof(si);

        memset(&procInf, 0, sizeof(procInf));

        memset(&securityAttributes, 0, sizeof(securityAttributes));
        securityAttributes.nLength = sizeof(securityAttributes);
        securityAttributes.lpSecurityDescriptor = NULL;
        securityAttributes.bInheritHandle = FALSE;

        bInheritHandles = FALSE;

        res = CreateProcessW(
                    NULL, 
                    command,
                    &securityAttributes,  // pointer to process security attributes 
                    &securityAttributes,  // pointer to thread security attributes 
                    bInheritHandles, // handle inheritance flag 
                    creation_flags, // creation flags 
                    NULL,  // pointer to new environment block 
                    NULL,  // pointer to current directory name 
                    &si,   // pointer to STARTUPINFO 
                    &procInf);  // pointer to PROCESS_INFORMATION  

        if (res == 0)
        {
            process_executed = FALSE;
            error_message_aux = _wcsdup(L"bsapicmdln.exe not found");
        }
        else
        {
            (void)WaitForSingleObject(procInf.hProcess, INFINITE);

            errorlevel = 0;
            res = GetExitCodeProcess(procInf.hProcess, &errorlevel);
        
            if (res != 0)
            {
                process_executed = TRUE;
                error_message_aux = NULL;
            }
            else
            {
                process_executed = FALSE;
                error_message_aux = _wcsdup(L"Unexpected error while executing api call");
            }
        }

        free(ApiExecutablePath);
        free(arguments_string);
        free(command);
    }

    *error_message = error_message_aux;

    return process_executed;
}

// ----------------------------------------------------------------------------------------------------

static void i_realloc_line_buffer(wchar_t **buffer, size_t *buffer_size)
{
    wchar_t *new_buffer;
    size_t new_buffer_size;

    assert(buffer != NULL && *buffer != NULL);
    assert(buffer_size != NULL);
    assert(*buffer_size > 0);

    new_buffer_size = *buffer_size + bsStringBuilderBufferSize;
    new_buffer = (wchar_t *)realloc(*buffer, new_buffer_size);
    assert(new_buffer != NULL);

    free(*buffer);
    *buffer = new_buffer;
    *buffer_size = new_buffer_size;
}

// ----------------------------------------------------------------------------------------------------

static void i_append_character_to_buffer(wchar_t character, size_t *pos, wchar_t **buffer, size_t *buffer_size)
{
    assert(buffer != NULL);
    assert(pos != NULL);
    assert(buffer_size != NULL);
    assert(*pos <= *buffer_size);

    if (*pos == *buffer_size)
        i_realloc_line_buffer(buffer, buffer_size);

    (*buffer)[(*pos)++] = character;
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_read_text_line(FILE *file)
{
    wchar_t *buffer;
    size_t buffer_size, pos;
    wint_t character;

    buffer_size = bsStringBuilderBufferSize;
    buffer = (wchar_t *)malloc(buffer_size * sizeof(wchar_t));

    pos = 0;

    while ((character = fgetwc(file)) != WEOF && character != L'\n')
        i_append_character_to_buffer(character, &pos, &buffer, &buffer_size);

    i_append_character_to_buffer(L'\0', &pos, &buffer, &buffer_size);

    return buffer;
}

// ----------------------------------------------------------------------------------------------------

static BIMserverAPIResponse *i_execute_api_call_and_get_results(
                        wchar_t **api_call_with_arguments, 
                        const wchar_t *api_name, 
                        const wchar_t *api_response_folder,
                        BOOL read_next_line_for_yes_response)
{
    BIMserverAPIResponse *response;
    wchar_t *error_message;

    assert(api_call_with_arguments != NULL && *api_call_with_arguments != NULL);

    if (i_did_execute_process(*api_call_with_arguments, api_response_folder, &error_message) == FALSE)
    {
        response = BIMserverAPIResponse_new_error_response(&error_message);
    }
    else
    {
        wchar_t *api_response_file_path;
        FILE *file;

        api_response_file_path = i_api_call_response_file_path(api_name, api_response_folder);
        file = _wfopen(api_response_file_path, L"rt, ccs=UNICODE");

        if (file == NULL)
        {
            error_message = _wcsdup(L"API response file not found");
            response = BIMserverAPIResponse_new_error_response(&error_message);
        }
        else
        {
            wchar_t *status;

            status = i_read_text_line(file);

            if (_wcsicmp(status, bsAPIResponse_TRUE) == 0)
            {
                if (read_next_line_for_yes_response == TRUE)
                {
                    wchar_t *next_line;

                    next_line = i_read_text_line(file);
                    response = BIMserverAPIResponse_new_true_response_with_response_text(&next_line);
                }
                else
                {
                    response = BIMserverAPIResponse_new_true_response();
                }
            }
            else if (_wcsicmp(status, bsAPIResponse_FALSE) == 0)
            {
                response = BIMserverAPIResponse_new_false_response();
            }
            else if (_wcsicmp(status, bsAPIResponse_BLOCKED_RESOURCE) == 0)
            {
                error_message = i_read_text_line(file);
                response = BIMserverAPIResponse_new_blocked_resource_response(&error_message);
            }
            else if (_wcsicmp(status, bsAPIResponse_ERROR) == 0)
            {
                error_message = i_read_text_line(file);
                response = BIMserverAPIResponse_new_error_response(&error_message);
            }
            else
            {
                error_message = _wcsdup(L"Unknown response type");
                response = BIMserverAPIResponse_new_error_response(&error_message);
            }

            free(status);
            fclose(file);
        }

        free(api_response_file_path);
    }

    free(*api_call_with_arguments);
    *api_call_with_arguments = NULL;

    return response;
}

// ----------------------------------------------------------------------------------------------------

static BIMserverAPIResponse *i_execute_api_call_and_get_status(
                        wchar_t **api_call_with_arguments, 
                        const wchar_t *api_name, 
                        const wchar_t *api_response_folder)
{
    BOOL read_next_line_for_yes_response;

    read_next_line_for_yes_response = FALSE;
    return i_execute_api_call_and_get_results(api_call_with_arguments, api_name, api_response_folder, read_next_line_for_yes_response);
}

// ----------------------------------------------------------------------------------------------------

static BIMserverAPIResponse *i_execute_api_call_and_get_status_plus_yes_response(
                        wchar_t **api_call_with_arguments, 
                        const wchar_t *api_name, 
                        const wchar_t *api_response_folder)
{
    BOOL read_next_line_for_yes_response;

    read_next_line_for_yes_response = TRUE;
    return i_execute_api_call_and_get_results(api_call_with_arguments, api_name, api_response_folder, read_next_line_for_yes_response);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_make_api_command_withoud_arguments(const wchar_t *api_call)
{
    return i_make_string_with_format(L"-%s", api_call);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_make_api_command_1_argument(const wchar_t *api_call, const wchar_t *argument1)
{
    return i_make_string_with_format(L"-%s %s", api_call, argument1);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_make_api_command_2_arguments(const wchar_t *api_call, const wchar_t *argument1, const wchar_t *argument2)
{
    return i_make_string_with_format(L"-%s %s %s", api_call, argument1, argument2);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_make_api_command_5_arguments(
                        const wchar_t *api_call, 
                        const wchar_t *argument1, const wchar_t *argument2, const wchar_t *argument3, 
                        const wchar_t *argument4, const wchar_t *argument5)
{
    return i_make_string_with_format(L"-%s %s %s %s %s %s", api_call, argument1, argument2, argument3, argument4, argument5);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_create_database(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_create_database);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_create_database, bsDataBaseFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_is_logged(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_is_logged);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_is_logged, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_loginForm(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_login);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_login, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_name(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_get_logged_user_name);
    return i_execute_api_call_and_get_status_plus_yes_response(&api_call_with_arguments, bsAPICall_get_logged_user_name, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_email(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_get_logged_user_email);
    return i_execute_api_call_and_get_status_plus_yes_response(&api_call_with_arguments, bsAPICall_get_logged_user_email, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_image(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_get_logged_user_image);
    return i_execute_api_call_and_get_status_plus_yes_response(&api_call_with_arguments, bsAPICall_get_logged_user_image, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_connect(const wchar_t *user_name, const wchar_t *user_password)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_2_arguments(bsAPICall_connect, user_name, user_password);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_connect, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_disconnect(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_disconnect);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_disconnect, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_send_user_recover_password_email(const wchar_t *user_email)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_1_argument(bsAPICall_send_user_recover_password_email, user_email);
    return i_execute_api_call_and_get_status(&api_call_with_arguments, bsAPICall_send_user_recover_password_email, bsLoginFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_select_current_project(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_select_current_project);

    return i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_select_current_project, 
                        bsDataBaseFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_get_current_project_path(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_get_current_project_path);

    return i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_get_current_project_path, 
                        bsDataBaseFolder);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_select_ifc_file_from_current_project(void)
{
    wchar_t *api_call_with_arguments;

    api_call_with_arguments = i_make_api_command_withoud_arguments(bsAPICall_select_ifc_file_from_current_project);

    return i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_select_ifc_file_from_current_project, 
                        bsDataBaseFolder);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_file_path_parameter(const wchar_t *absolute_file_path)
{
    return i_make_string_with_format(L"\"%s\"", absolute_file_path);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_put_file_in_current_project(const wchar_t *absolute_file_path)
{
    BIMserverAPIResponse *response;
    wchar_t *absolute_file_path_parameter;
    wchar_t *api_call_with_arguments;

    absolute_file_path_parameter = i_file_path_parameter(absolute_file_path);
    api_call_with_arguments = i_make_api_command_1_argument(bsAPICall_put_file_in_current_project, absolute_file_path_parameter);

    response = i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_put_file_in_current_project, 
                        bsDataBaseFolder);

    free(absolute_file_path_parameter);

    return response;
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_select_associated_file_from_current_project(const wchar_t *extension_filter)
{
    BIMserverAPIResponse *response;
    wchar_t *extension_filter_parameter;
    wchar_t *api_call_with_arguments;

    extension_filter_parameter = i_get_filter_by_extension_parameter(extension_filter);
    api_call_with_arguments = i_make_api_command_1_argument(bsAPICall_select_associated_file_from_current_project, extension_filter_parameter);

    response = i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_select_associated_file_from_current_project, 
                        bsDataBaseFolder);

    free(extension_filter_parameter);

    return response;
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_optional_parameter(const wchar_t *parameter, const wchar_t *parameter_value)
{
    if (parameter_value == NULL || wcslen(parameter_value) == 0)
        return _wcsdup(L"");
    else
        return i_make_string_with_format(L"-%s \"%s\"", parameter, parameter_value);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_put_associated_file_from_current_project(
                        const wchar_t *absolute_file_path,
                        const wchar_t *file_description, 
                        const wchar_t *app_name, const wchar_t *app_version, const wchar_t *app_description)
{
    BIMserverAPIResponse *response;
    wchar_t *absolute_file_path_parameter;
    wchar_t *file_description_parameter, *app_name_parameter, *app_version_parameter, *app_desc_parameter;
    wchar_t *api_call_with_arguments;

    absolute_file_path_parameter = i_file_path_parameter(absolute_file_path);
    file_description_parameter = i_get_optional_parameter(L"file_desc", file_description);
    app_name_parameter = i_get_optional_parameter(L"app_name", app_name);
    app_version_parameter = i_get_optional_parameter(L"app_version", app_version);
    app_desc_parameter = i_get_optional_parameter(L"app_desc", app_description);

    api_call_with_arguments = i_make_api_command_5_arguments(
                        bsAPICall_put_associated_file_in_current_project, 
                        absolute_file_path_parameter,
                        file_description_parameter,
                        app_name_parameter, app_version_parameter, app_desc_parameter);

    response = i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_put_associated_file_in_current_project, 
                        bsDataBaseFolder);

    free(absolute_file_path_parameter);
    free(file_description_parameter);
    free(app_name_parameter);
    free(app_version_parameter);
    free(app_desc_parameter);

    return response;
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_convert_relative_project_file_path_to_absolute(const wchar_t *relative_file_path_to_project, int update_database_time_stamp)
{
    BIMserverAPIResponse *response;
    wchar_t *relative_file_path_parameter;
    wchar_t *update_database_timestamp_parameter;
    wchar_t *api_call_with_arguments;

    relative_file_path_parameter = i_file_path_parameter(relative_file_path_to_project);
    update_database_timestamp_parameter = i_get_update_database_timestamp_parameter(update_database_time_stamp);
    api_call_with_arguments = i_make_api_command_2_arguments(bsAPICall_get_file_path_in_current_project, relative_file_path_parameter, update_database_timestamp_parameter);

    response = i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_get_file_path_in_current_project, 
                        bsDataBaseFolder);

    free(relative_file_path_parameter);
    free(update_database_timestamp_parameter);

    return response;
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIWrapper_exists_updated_file_version_current_project(const wchar_t *relative_file_path_to_project)
{
    BIMserverAPIResponse *response;
    wchar_t *relative_file_path_parameter;
    wchar_t *api_call_with_arguments;

    relative_file_path_parameter = i_file_path_parameter(relative_file_path_to_project);
    api_call_with_arguments = i_make_api_command_1_argument(bsAPICall_exists_updated_file_version_current_project, relative_file_path_parameter);

    response = i_execute_api_call_and_get_status_plus_yes_response(
                        &api_call_with_arguments, 
                        bsAPICall_exists_updated_file_version_current_project, 
                        bsDataBaseFolder);

    free(relative_file_path_parameter);

    return response;
}