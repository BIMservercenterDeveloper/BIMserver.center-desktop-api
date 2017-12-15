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

#include "BIMserverAPIResponse.hxx"
#include "BIMserverAPIWrapper.hxx"
#include <wchar.h>
#include <windows.h>

#ifdef  __cplusplus
extern "C" {
#endif

// API configuration...

BOOL BIMserverAPIWrapper_is_api_installed(void);

void BIMserverAPIWrapper_set_database_path(const wchar_t *database_folder);

void BIMserverAPIWrapper_set_login_path(const wchar_t *login_folder);

void BIMserverAPIWrapper_set_language(BIMserverAPILanguage language);


// Database...

BIMserverAPIResponse *BIMserverAPIWrapper_create_database(void);


// Login...

BIMserverAPIResponse *BIMserverAPIWrapper_is_logged(void);

BIMserverAPIResponse *BIMserverAPIWrapper_loginForm(void);

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_name(void);

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_email(void);

BIMserverAPIResponse *BIMserverAPIWrapper_get_logged_user_image(void);

BIMserverAPIResponse *BIMserverAPIWrapper_connect(const wchar_t *user_name, const wchar_t *user_password);

BIMserverAPIResponse *BIMserverAPIWrapper_disconnect(void);

BIMserverAPIResponse *BIMserverAPIWrapper_send_user_recover_password_email(const wchar_t *user_email);


// Current project management...

BIMserverAPIResponse *BIMserverAPIWrapper_select_current_project(void);

BIMserverAPIResponse *BIMserverAPIWrapper_get_current_project_path(void);

BIMserverAPIResponse *BIMserverAPIWrapper_select_ifc_file_from_current_project(void);

BIMserverAPIResponse *BIMserverAPIWrapper_put_file_in_current_project(const wchar_t *absolute_file_path);

BIMserverAPIResponse *BIMserverAPIWrapper_select_associated_file_from_current_project(const wchar_t *extension_filter);

BIMserverAPIResponse *BIMserverAPIWrapper_put_associated_file_from_current_project(
                        const wchar_t *absolute_file_path,
                        const wchar_t *file_description, 
                        const wchar_t *app_name, const wchar_t *app_version, const wchar_t *app_description);

BIMserverAPIResponse *BIMserverAPIWrapper_convert_relative_project_file_path_to_absolute(const wchar_t *relative_file_path_to_project, int update_database_time_stamp);

BIMserverAPIResponse *BIMserverAPIWrapper_exists_updated_file_version_current_project(const wchar_t *relative_file_path_to_project);


#ifdef  __cplusplus
}
#endif
