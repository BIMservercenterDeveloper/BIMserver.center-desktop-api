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

#include "stdafx.h"
#include <Windows.h>

#include "BIMserverAPIResponse.h"
#include "BIMserverAPIWrapper.h"

#include <assert.h>
#include <stdio.h>
#include <wchar.h>

HINSTANCE hinst = NULL; 
HFONT global_font = NULL;

static const wchar_t *i_MAIN_WINDOW_CLASS_NAME = L"bsTestAPIWindowClass";
static const int i_MAIN_WINDOW_WIDTH = 300;
static const int i_MAIN_WINDOW_HEIGHT = 600;

static const wchar_t *i_LOGIN_DATA_WINDOW_CLASS_NAME = L"bsLoginDataWindowClass";
static const int i_LOGIN_DATA_WINDOW_WIDTH = 300;
static const int i_LOGIN_DATA_WINDOW_HEIGHT = 300;

static const int i_IMAGE_SIZE = 64;

#define i_ID_CREATE_DATABASE                            1000
#define i_ID_LOGIN                                      1001
#define i_ID_CHECK_LOGIN_STATUS                         1002
#define i_ID_SELECT_CURRENT_PROJECT                     1003
#define i_ID_SELECT_IFC_FROM_CURRENT_PROJECT            1004
#define i_ID_SELECT_ASSOCIATED_FROM_CURRENT_PROJECT     1005
#define i_ID_ABSOLUTE_FILE_PATHS                        1006
#define i_ID_PUT_IFC_FILE_INTO_CURRENT_PROJECT          1007
#define i_ID_PUT_ASSOCIATED_FILE_INTO_CURRENT_PROJECT   1008
#define i_ID_SHOW_CURRENT_PROJECT_PATH                  1009
#define i_ID_CHECK_UPDATED_FILE                         1010
#define i_ID_SHOW_LOGIN_DATA                            1011

static wchar_t *g_selected_ifc_file_from_current_project = NULL;
static wchar_t *g_selected_associated_file_from_current_project = NULL;

static wchar_t *g_user_logged_name = NULL;
static wchar_t *g_user_logged_email = NULL;
static wchar_t *g_user_logged_image = NULL;
static HBITMAP g_use_image = NULL;

// ----------------------------------------------------------------------------------------------------

static void i_print_message(HWND parent_window_hwnd, const wchar_t *message)
{
    (void)MessageBoxW(parent_window_hwnd, message, L"", MB_OK);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_concat_strings(const wchar_t *s1, const wchar_t *s2)
{
    size_t buffer_size;
    wchar_t *buffer;

    buffer_size = wcslen(s1) + wcslen(s2) + 1;
    buffer = (wchar_t *)calloc(buffer_size, sizeof(wchar_t));

    lstrcpyW(buffer, s1);
    lstrcatW(buffer, s2);
    buffer[buffer_size - 1] = L'\0';

    return buffer;
}

// ----------------------------------------------------------------------------------------------------

static void i_concat_strings_pp(wchar_t **result, const wchar_t *s2)
{
    wchar_t *result_aux;

    assert(result != NULL);

    result_aux = i_concat_strings(*result, s2);

    free(*result);
    *result = result_aux;
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_create_database(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_create_database();
    assert(response != NULL);

    if (response->type == BIMserverAPIResponseType_TRUE)
    {
        i_print_message(parent_window_hwnd, L"BIMserverAPI Database created");
    }
    else
    {
        assert(response->type == BIMserverAPIResponseType_ERROR);
        i_print_message(parent_window_hwnd, response->response_result);
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_login_form(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_loginForm();
    assert(response != NULL);

    switch (response->type)
    {
        case BIMserverAPIResponseType_TRUE:             i_print_message(parent_window_hwnd, L"User is logged");      break;
        case BIMserverAPIResponseType_FALSE:            i_print_message(parent_window_hwnd, L"User is not logged");  break;
        case BIMserverAPIResponseType_BLOCKED_RESOURCE: i_print_message(parent_window_hwnd, response->response_result); break;
        case BIMserverAPIResponseType_ERROR:            i_print_message(parent_window_hwnd, response->response_result); break;
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_is_logged(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_is_logged();
    assert(response != NULL);
    assert(response->type != BIMserverAPIResponseType_BLOCKED_RESOURCE);

    switch (response->type)
    {
        case BIMserverAPIResponseType_TRUE: i_print_message(parent_window_hwnd, L"User is logged"); break;
        case BIMserverAPIResponseType_FALSE: i_print_message(parent_window_hwnd, L"User is not logged"); break;
        case BIMserverAPIResponseType_ERROR: i_print_message(parent_window_hwnd, response->response_result); break;
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_select_current_project(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_select_current_project();
    assert(response != NULL);
            
    switch (response->type)
    {
        case BIMserverAPIResponseType_TRUE:                 i_print_message(parent_window_hwnd, response->response_result); break;
        case BIMserverAPIResponseType_FALSE:                i_print_message(parent_window_hwnd, L"No project selected or changed"); break;
        case BIMserverAPIResponseType_BLOCKED_RESOURCE:     i_print_message(parent_window_hwnd, response->response_result); break;
        case BIMserverAPIResponseType_ERROR:                i_print_message(parent_window_hwnd, response->response_result); break;
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_select_ifc_from_current_project(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_select_ifc_file_from_current_project();
    assert(response != NULL);
    
    switch (response->type)
    {
        case BIMserverAPIResponseType_TRUE:
        {    
            i_print_message(parent_window_hwnd, response->response_result);

            if (g_selected_ifc_file_from_current_project != NULL)
                free(g_selected_ifc_file_from_current_project);

            g_selected_ifc_file_from_current_project = _wcsdup(response->response_result);
            break;
        }

        case BIMserverAPIResponseType_FALSE:

            i_print_message(parent_window_hwnd, L"No file selected");
            break;

        case BIMserverAPIResponseType_ERROR:

            i_print_message(parent_window_hwnd, response->response_result);
            break;
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_select_associated_file_from_current_project(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_select_associated_file_from_current_project(L"txt;docx;pdf;dwg;dxf");
    assert(response->type != BIMserverAPIResponseType_BLOCKED_RESOURCE);

    switch (response->type)
    {
        case BIMserverAPIResponseType_TRUE:
        {
            i_print_message(parent_window_hwnd, response->response_result);

            if (g_selected_associated_file_from_current_project != NULL)
                free(g_selected_associated_file_from_current_project);

            g_selected_associated_file_from_current_project = _wcsdup(response->response_result);
            break;
        }

        case BIMserverAPIResponseType_FALSE:

            i_print_message(parent_window_hwnd, L"No file selected");
            break;

        case BIMserverAPIResponseType_ERROR:

            i_print_message(parent_window_hwnd, response->response_result);
            break;
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_convert_relative_project_file_path_to_absolute(const wchar_t *project_relative_file_path, BOOL update_database_time_stamp)
{
    wchar_t *absolute_file_path;

    if (project_relative_file_path == NULL)
    {
        absolute_file_path = _wcsdup(L"No IFC file was selected");
    }
    else
    {
        BIMserverAPIResponse *response;
        
        response = BIMserverAPIWrapper_convert_relative_project_file_path_to_absolute(project_relative_file_path, update_database_time_stamp);
        assert(response->type == BIMserverAPIResponseType_TRUE || response->type == BIMserverAPIResponseType_ERROR);

        absolute_file_path = _wcsdup(response->response_result);

        BIMserverAPIResponse_free(&response);
    }

    return absolute_file_path;
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_convert_relative_project_file_path_to_absolute(HWND parent_window_hwnd)
{
    BOOL update_database_time_stamp;
    wchar_t *absoluteIfcFilePath, *absoluteFilePathAssociatedFile;
    wchar_t *message;

    update_database_time_stamp = TRUE;

    absoluteIfcFilePath = i_convert_relative_project_file_path_to_absolute(g_selected_ifc_file_from_current_project, update_database_time_stamp);
    absoluteFilePathAssociatedFile = i_convert_relative_project_file_path_to_absolute(g_selected_associated_file_from_current_project, update_database_time_stamp);

    message = _wcsdup(L"");
    i_concat_strings_pp(&message, L"IFC file: ");
    i_concat_strings_pp(&message, absoluteIfcFilePath);
    i_concat_strings_pp(&message, L"\n");
    i_concat_strings_pp(&message, L"Associated file: ");
    i_concat_strings_pp(&message, absoluteFilePathAssociatedFile);

    i_print_message(parent_window_hwnd, message);

    free(absoluteIfcFilePath);
    free(absoluteFilePathAssociatedFile);
    free(message);
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_put_file_in_current_project(HWND parent_window_hwnd)
{
    OPENFILENAME ofn;
    WCHAR file_name[MAX_PATH];

    ZeroMemory(&ofn, sizeof(ofn));
    ofn.lStructSize = sizeof(ofn);
    ofn.hwndOwner = parent_window_hwnd;
    ofn.lpstrFile = file_name;

    ofn.lpstrFile[0] = '\0';
    ofn.nMaxFile = sizeof(file_name);
    ofn.lpstrFilter = L"IFC Files (*.ifc)\0*.ifc\0";
    ofn.nFilterIndex = 1;
    ofn.lpstrFileTitle = NULL;
    ofn.nMaxFileTitle = 0;
    ofn.lpstrInitialDir = NULL;
    ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

    if (GetOpenFileName(&ofn) == TRUE)
    {
        BIMserverAPIResponse *response;
        
        response = BIMserverAPIWrapper_put_file_in_current_project(ofn.lpstrFile);

        if (response->type == BIMserverAPIResponseType_TRUE)
        {
            i_print_message(parent_window_hwnd, L"File added correctly");
        }
        else
        {
            assert(response->type == BIMserverAPIResponseType_ERROR);
            i_print_message(parent_window_hwnd, response->response_result);
        }

        BIMserverAPIResponse_free(&response);
    }
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_put_associated_file_into_current_project(HWND parent_window_hwnd)
{
    OPENFILENAME ofn;
    WCHAR file_name[MAX_PATH];

    ZeroMemory(&ofn, sizeof(ofn));
    ofn.lStructSize = sizeof(ofn);
    ofn.hwndOwner = parent_window_hwnd;
    ofn.lpstrFile = file_name;

    ofn.lpstrFile[0] = '\0';
    ofn.nMaxFile = sizeof(file_name);
    ofn.lpstrFilter = L"Text files (*.txt)\0*.txt\0PDF files (*.pdf)\0*.pdf\0";
    ofn.nFilterIndex = 1;
    ofn.lpstrFileTitle = NULL;
    ofn.nMaxFileTitle = 0;
    ofn.lpstrInitialDir = NULL;
    ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

    if (GetOpenFileName(&ofn) == TRUE)
    {
        BIMserverAPIResponse *response;

        response = BIMserverAPIWrapper_put_associated_file_from_current_project(
                        ofn.lpstrFile, 
                        L"An associated file",
                        L"BIMserver.center demo test api", L"0.1", L"API Demo tools");

        if (response->type == BIMserverAPIResponseType_TRUE)
        {
            i_print_message(parent_window_hwnd, L"File added correctly");
        }
        else
        {
            assert(response->type == BIMserverAPIResponseType_ERROR);
            i_print_message(parent_window_hwnd, response->response_result);
        }

        BIMserverAPIResponse_free(&response);
    }
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_get_current_project_path(HWND parent_window_hwnd)
{
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_get_current_project_path();
    assert(response != NULL);

    if (response->type == BIMserverAPIResponseType_TRUE)
    {
        i_print_message(parent_window_hwnd, response->response_result);
    }
    else
    {
        assert(response->type == BIMserverAPIResponseType_ERROR);
        i_print_message(parent_window_hwnd, response->response_result);
    }

    BIMserverAPIResponse_free(&response);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_exists_updated_version_of_file(const wchar_t *relative_file_path_to_project)
{
    wchar_t *result;

    if (relative_file_path_to_project == NULL)
    {
        result = _wcsdup(L"No file selected");
    }
    else
    {
        BIMserverAPIResponse *response;
        
        response = BIMserverAPIWrapper_exists_updated_file_version_current_project(relative_file_path_to_project);
        assert(response->type != BIMserverAPIResponseType_BLOCKED_RESOURCE);

        switch (response->type)
        {
            case BIMserverAPIResponseType_TRUE:

                result = _wcsdup(L"YES");
                break;

            case BIMserverAPIResponseType_FALSE:

                result = _wcsdup(L"NO");
                break;

            default:

                assert(response->type == BIMserverAPIResponseType_ERROR);
                result = _wcsdup(response->response_result);
                break;
        }

        BIMserverAPIResponse_free(&response);
    }

    return result;
}

// ----------------------------------------------------------------------------------------------------

static void i_demo_check_updated_file(HWND parent_window_hwnd)
{
    wchar_t *updateIfcFileStatus, *updateAssociatedFileStatus;
    wchar_t *message;
    
    updateIfcFileStatus = i_exists_updated_version_of_file(g_selected_ifc_file_from_current_project);
    updateAssociatedFileStatus = i_exists_updated_version_of_file(g_selected_associated_file_from_current_project);

    message = _wcsdup(L"");
    i_concat_strings_pp(&message, L"IFC file: ");
    i_concat_strings_pp(&message, updateIfcFileStatus);
    i_concat_strings_pp(&message, L"\n");
    i_concat_strings_pp(&message, L"Associated file: ");
    i_concat_strings_pp(&message, updateAssociatedFileStatus);

    i_print_message(parent_window_hwnd, message);

    free(updateIfcFileStatus);
    free(updateAssociatedFileStatus);
    free(message);
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_user_logged_name(void)
{
    wchar_t *user_data;
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_get_logged_user_name();
    assert(response->type == BIMserverAPIResponseType_TRUE || response->type == BIMserverAPIResponseType_ERROR);

    user_data = _wcsdup(L"User name: ");
    i_concat_strings_pp(&user_data, response->response_result);

    BIMserverAPIResponse_free(&response);

    return user_data;
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_user_logged_email(void)
{
    wchar_t *user_data;
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_get_logged_user_email();
    assert(response->type == BIMserverAPIResponseType_TRUE || response->type == BIMserverAPIResponseType_ERROR);

    user_data = _wcsdup(L"Email: ");
    i_concat_strings_pp(&user_data, response->response_result);

    BIMserverAPIResponse_free(&response);

    return user_data;
}

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_get_user_logged_image_as_path(void)
{
    wchar_t *user_data;
    BIMserverAPIResponse *response;

    response = BIMserverAPIWrapper_get_logged_user_image();
    assert(response->type == BIMserverAPIResponseType_TRUE || response->type == BIMserverAPIResponseType_ERROR);

    user_data = _wcsdup(response->response_result);

    BIMserverAPIResponse_free(&response);

    return user_data;
}

// ----------------------------------------------------------------------------------------------------

static void i_append_text_label_to_window(const wchar_t *text, HWND parent_window, int x_pos, int y_pos, int width, int heigth)
{
    HWND label_hwnd; 

    label_hwnd = CreateWindow( 
                    L"STATIC",
                    text,
                    WS_CHILD | WS_VISIBLE | SS_LEFT,
                    x_pos, y_pos, width, heigth,
                    parent_window,
                    (HMENU)NULL,
                    (HINSTANCE)GetWindowLong(parent_window, GWL_HINSTANCE),
                    (LPVOID)NULL);

    (void)SendMessage(label_hwnd, (UINT) WM_SETFONT, (WPARAM)global_font, (LPARAM)TRUE);
}

// ----------------------------------------------------------------------------------------------------

static void i_append_user_image_to_window(HBITMAP user_image, HWND parent_window, int x_pos, int y_pos, int width, int heigth)
{
    HWND image_hwnd; 

    image_hwnd = CreateWindow( 
                    L"STATIC",
                    L"",
                    WS_CHILD | WS_VISIBLE | SS_BITMAP,
                    x_pos, y_pos, width, heigth,
                    parent_window,
                    (HMENU)NULL,
                    (HINSTANCE)GetWindowLong(parent_window, GWL_HINSTANCE),
                    (LPVOID)NULL);

    (void)SendMessage(image_hwnd, (UINT)STM_SETIMAGE, (WPARAM)IMAGE_BITMAP, (LPARAM)user_image);
}

// ----------------------------------------------------------------------------------------------------

static LRESULT CALLBACK i_login_data_window_procedure(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{ 
	switch (message)
	{
        case WM_CREATE:
        {
            int x_pos, y_pos, width, heigth, y_margin;

            x_pos = 10;
            y_pos = 10;
            width = i_LOGIN_DATA_WINDOW_WIDTH - 20;
            heigth = 14;
            y_margin = heigth + 8;

            i_append_user_image_to_window(g_use_image, hWnd, x_pos, y_pos, i_IMAGE_SIZE, i_IMAGE_SIZE);
            y_pos += i_IMAGE_SIZE + heigth; 

            i_append_text_label_to_window(g_user_logged_name, hWnd, x_pos, y_pos, width, heigth);
            y_pos += y_margin; 

            i_append_text_label_to_window(g_user_logged_email, hWnd, x_pos, y_pos, width, heigth);
            break;
        }

	    case WM_PAINT:
        {
	        PAINTSTRUCT ps;
	        HDC hdc;

		    hdc = BeginPaint(hWnd, &ps);		    
		    EndPaint(hWnd, &ps);
		    break;
        }

	    case WM_DESTROY:
        {
		    PostQuitMessage(0);
		    break;
        }

	    default:

		    return DefWindowProc(hWnd, message, wParam, lParam);
	}

	return 0;
}

// ----------------------------------------------------------------------------------------------------

static int i_process_window_messages(void)
{
    MSG msg;
    BOOL fGotMessage;

    while ((fGotMessage = GetMessage(&msg, (HWND) NULL, 0, 0)) != 0 && fGotMessage != -1) 
    { 
        TranslateMessage(&msg); 
        DispatchMessage(&msg); 
    } 

    return msg.wParam; 
}

// ----------------------------------------------------------------------------------------------------

static BOOL i_register_login_data_window(HINSTANCE hinstance) 
{ 
    WNDCLASSEX wcx; 
 
    wcx.cbSize = sizeof(wcx);
    wcx.style = CS_HREDRAW | CS_VREDRAW;
    wcx.lpfnWndProc = i_login_data_window_procedure;
    wcx.cbClsExtra = 0;
    wcx.cbWndExtra = 0;
    wcx.hInstance = hinstance;
    wcx.hIcon = NULL;
    wcx.hCursor = NULL;
    wcx.hbrBackground = (HBRUSH)GetSysColorBrush(COLOR_3DFACE);
    wcx.lpszMenuName =  L"LoginMenu";
    wcx.lpszClassName = i_LOGIN_DATA_WINDOW_CLASS_NAME;
    wcx.hIconSm = NULL; 
    
    return RegisterClassEx(&wcx); 
}
 
// ----------------------------------------------------------------------------------------------------

static void i_demo_show_login_data(HWND parent_hwnd)
{ 
    HWND login_data_hwnd; 

    (void)i_register_login_data_window(hinst);

    g_user_logged_name = i_get_user_logged_name();
    g_user_logged_email = i_get_user_logged_email();
    g_user_logged_image = i_get_user_logged_image_as_path();
    g_use_image = (HBITMAP)LoadImage(NULL, g_user_logged_image, IMAGE_BITMAP, 64, 64, LR_LOADFROMFILE);

    login_data_hwnd = CreateWindow( 
                    i_LOGIN_DATA_WINDOW_CLASS_NAME,
                    L"ShowLoginDataForm",
                    WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME,
                    CW_USEDEFAULT,       // default horizontal position 
                    CW_USEDEFAULT,       // default vertical position 
                    i_LOGIN_DATA_WINDOW_WIDTH, 
                    i_LOGIN_DATA_WINDOW_HEIGHT,
                    (HWND)parent_hwnd,
                    (HMENU) NULL,        // use class menu 
                    hinst,
                    (LPVOID) NULL);      // no window-creation data 
 
    if (login_data_hwnd != NULL) 
    {
        ShowWindow(login_data_hwnd, SW_SHOWNORMAL); 
        UpdateWindow(login_data_hwnd); 

        (void)i_process_window_messages();

        (void)CloseWindow(login_data_hwnd);
        (void)DestroyWindow(login_data_hwnd); 
    }

    free(g_user_logged_name);
    free(g_user_logged_email);
    free(g_user_logged_image);
    DeleteObject(g_use_image);
} 

// ----------------------------------------------------------------------------------------------------

static void i_append_button(const WCHAR *text, int button_id, int x_pos, int *y_pos, int width, int heigth, int y_pos_incr, HWND parent_window)
{
    HWND button_hwnd; 

    assert(y_pos != NULL);

    button_hwnd = CreateWindow( 
                    L"BUTTON",
                    text,
                    WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON,
                    x_pos, *y_pos, width, heigth,
                    parent_window,
                    (HMENU)button_id,
                    (HINSTANCE)GetWindowLong(parent_window, GWL_HINSTANCE),
                    (LPVOID)NULL);

    (void)SendMessage(button_hwnd, (UINT) WM_SETFONT, (WPARAM)global_font, (LPARAM)TRUE);

    *y_pos += y_pos_incr;
}

// ----------------------------------------------------------------------------------------------------

static LRESULT CALLBACK i_main_window_procedure(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{ 
	switch (message)
	{
        case WM_CREATE:
        {
            int x_pos, y_pos;
            int width, heigth, y_pos_incr;

            x_pos = 10;
            y_pos = 10;
            width = 260;
            heigth = 36;
            y_pos_incr = heigth + 8;

            i_append_button(L"Create database", i_ID_CREATE_DATABASE, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Login", i_ID_LOGIN, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Check login status", i_ID_CHECK_LOGIN_STATUS, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Select current project", i_ID_SELECT_CURRENT_PROJECT, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Select IFC file from current project", i_ID_SELECT_IFC_FROM_CURRENT_PROJECT, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Select associated file from current project", i_ID_SELECT_ASSOCIATED_FROM_CURRENT_PROJECT, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Absolute file paths", i_ID_ABSOLUTE_FILE_PATHS, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Put IFC file into current project", i_ID_PUT_IFC_FILE_INTO_CURRENT_PROJECT, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Put associated file into current project", i_ID_PUT_ASSOCIATED_FILE_INTO_CURRENT_PROJECT, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Show current project path", i_ID_SHOW_CURRENT_PROJECT_PATH, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Check updated file", i_ID_CHECK_UPDATED_FILE, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            i_append_button(L"Show login data", i_ID_SHOW_LOGIN_DATA, x_pos, &y_pos, width, heigth, y_pos_incr, hWnd);
            break;
        }

	    case WM_COMMAND:
        {
            int wmId, wmEvent;

		    wmId    = LOWORD(wParam);
		    wmEvent = HIWORD(wParam);

            switch (wmId)
            {
                case i_ID_CREATE_DATABASE:

                    i_demo_create_database(hWnd);
                    break;

                case i_ID_LOGIN:

                    i_demo_login_form(hWnd);
                    break;

                case i_ID_CHECK_LOGIN_STATUS:

                    i_demo_is_logged(hWnd);
                    break;

                case i_ID_SELECT_CURRENT_PROJECT:

                    i_demo_select_current_project(hWnd);
                    break;

                case i_ID_SELECT_IFC_FROM_CURRENT_PROJECT:

                    i_demo_select_ifc_from_current_project(hWnd);
                    break;

                case i_ID_SELECT_ASSOCIATED_FROM_CURRENT_PROJECT:

                    i_demo_select_associated_file_from_current_project(hWnd);
                    break;

                case i_ID_ABSOLUTE_FILE_PATHS:

                    i_demo_convert_relative_project_file_path_to_absolute(hWnd);
                    break;

                case i_ID_PUT_IFC_FILE_INTO_CURRENT_PROJECT:

                    i_demo_put_file_in_current_project(hWnd);
                    break;

                case i_ID_PUT_ASSOCIATED_FILE_INTO_CURRENT_PROJECT:

                    i_demo_put_associated_file_into_current_project(hWnd);
                    break;

                case i_ID_SHOW_CURRENT_PROJECT_PATH:

                    i_demo_get_current_project_path(hWnd);
                    break;

                case i_ID_CHECK_UPDATED_FILE:

                    i_demo_check_updated_file(hWnd);
                    break;

                case i_ID_SHOW_LOGIN_DATA:

                    i_demo_show_login_data(hWnd);
                    break;

                default:

                    return DefWindowProc(hWnd, message, wParam, lParam);
            }
        }

	    case WM_PAINT:
        {
	        PAINTSTRUCT ps;
	        HDC hdc;

		    hdc = BeginPaint(hWnd, &ps);		    
		    EndPaint(hWnd, &ps);
		    break;
        }

	    case WM_DESTROY:
        {
		    PostQuitMessage(0);
		    break;
        }

	    default:

		    return DefWindowProc(hWnd, message, wParam, lParam);
	}

	return 0;
}

// ----------------------------------------------------------------------------------------------------

static BOOL i_register_main_window(HINSTANCE hinstance) 
{ 
    WNDCLASSEX wcx; 
 
    wcx.cbSize = sizeof(wcx);
    wcx.style = CS_HREDRAW | CS_VREDRAW;
    wcx.lpfnWndProc = i_main_window_procedure;
    wcx.cbClsExtra = 0;
    wcx.cbWndExtra = 0;
    wcx.hInstance = hinstance;
    wcx.hIcon = NULL;
    wcx.hCursor = NULL;
    wcx.hbrBackground = (HBRUSH)GetSysColorBrush(COLOR_3DFACE);
    wcx.lpszMenuName =  L"MainMenu";
    wcx.lpszClassName = i_MAIN_WINDOW_CLASS_NAME;
    wcx.hIconSm = NULL; 
    
    return RegisterClassEx(&wcx); 
}
 
// ----------------------------------------------------------------------------------------------------

static BOOL i_init_instance(HINSTANCE hinstance, int nCmdShow) 
{ 
    HWND hwnd; 

    hinst = hinstance; 
 
    hwnd = CreateWindow( 
                    i_MAIN_WINDOW_CLASS_NAME,
                    L"Test API Methods - C",
                    WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME,
                    CW_USEDEFAULT,       // default horizontal position 
                    CW_USEDEFAULT,       // default vertical position 
                    i_MAIN_WINDOW_WIDTH, 
                    i_MAIN_WINDOW_HEIGHT,
                    (HWND) NULL,         // no owner window 
                    (HMENU) NULL,        // use class menu 
                    hinstance,           // handle to application instance 
                    (LPVOID) NULL);      // no window-creation data 
 
    if (!hwnd) 
    {
        return FALSE;
    }
    else
    {
        ShowWindow(hwnd, nCmdShow); 
        UpdateWindow(hwnd); 

        return TRUE;  
    }
} 

// ----------------------------------------------------------------------------------------------------

static void i_initialize_bimserver_api(void)
{
    BIMserverAPIWrapper_set_language(BIMserverAPILanguage_ENGLISH);
    BIMserverAPIWrapper_set_database_path(L"c:\\BIMserver.center api demo");
    BIMserverAPIWrapper_set_login_path(L"c:\\BIMserver.center api demo");

// This values must be set. You'll get them after registration.
    //BIMserverAPIWrapper_set_app_id(L"<undefined_app_id>");
    //BIMserverAPIWrapper_set_developer_id(L"<undefined_developer_id>");
}

// ----------------------------------------------------------------------------------------------------

static void i_set_font(void)
{
    int nHeight;

    nHeight = -MulDiv(8, GetDeviceCaps(GetDC(NULL), LOGPIXELSY), 72);
    global_font = CreateFont(nHeight, 0, 0, 0, 0, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, L"Microsoft Sans Serif");
}

// ----------------------------------------------------------------------------------------------------

int WINAPI WinMain(HINSTANCE hinstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow) 
{   
    UNREFERENCED_PARAMETER(lpCmdLine); 

    i_set_font();

    if (BIMserverAPIWrapper_is_api_installed() == FALSE)
    {
        i_print_message(NULL, L"API not found");
    }
    else
    {
        i_initialize_bimserver_api();

        if (!i_register_main_window(hinstance)) 
            return FALSE; 

        if (!i_init_instance(hinstance, nCmdShow)) 
            return FALSE; 

        return i_process_window_messages();
    }
}  
