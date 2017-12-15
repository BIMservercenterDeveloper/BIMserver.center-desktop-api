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

using System;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BIMserverAPI
{
    public enum BIMserverAPILanguage
    {
        SPANISH,
        ENGLISH,
        FRENCH,
        ITALIAN,
        DEUSTCH
    };

    /// <summary>
    /// BIMserver.center API is accessed through an independent executable file that manages a database associate to a project of your software.
    /// For every model of your software associated to a BIMserver.center project you will need an independent API database.
    /// 
    /// API calls are accessed through a command line argument and its respectives arguments are specified in the same way.
    /// The API response is written to a file with the name api_call_name.txt.
    /// The file is composed of several text lines, one parameter at each. The possible response formats are: ERROR, NO or YES.
    /// 
    /// Error response: Raised when API call fails
    /// --------------------------
    /// 
    /// ERROR
    /// Error description in one line
    /// end-of-file
    /// 
    /// Resource blocked: API couldn't perform because resouce was in use. I.E: Two simultaneous login in the same database.
    /// --------------------------
    /// 
    /// BLOCKED_RESOURCE
    /// Error description in one line. It can be an empty string.
    /// end-of-file
    /// 
    /// NO: The user has cancelled a dialog or API return has a YES/NO semantics
    /// -----------------------------
    /// NO
    /// end-of-file
    /// 
    /// YES: The user accepted a dialog or API return has a YES/NO semantics
    /// -----------------------------
    /// YES
    /// -API dependent response, could extend through several lines. Check the documentation.-
    /// end-of-file
    /// </summary>
    public static class BIMserverAPIWrapper
    {
        private static bool bsDebugMode = false;
        private const string bsApiExecutableName = "bsapicmdln.exe";

        private static string bsDataBaseFolder = null;
        private static string bsLoginFolder = null;

        private static string bsLanguage = "EN"; // ES, EN, FR, IT, DE

        #region API Calls

        private const string bsAPICall_create_database = "create_database";
        private const string bsAPICall_is_logged = "is_logged";
        private const string bsAPICall_login = "login";

        private const string bsAPICall_get_logged_user_name = "get_logged_user_name";
        private const string bsAPICall_get_logged_user_email = "get_logged_user_email";
        private const string bsAPICall_get_logged_user_image = "get_logged_user_image";

        private const string bsAPICall_connect = "connect";
        private const string bsAPICall_disconnect = "disconnect";
        private const string bsAPICall_send_user_recover_password_email = "send_user_recover_password_email";

        private const string bsAPICall_select_current_project = "select_current_project";
        private const string bsAPICall_get_current_project_path = "get_current_project_path";

        private const string bsAPICall_select_ifc_file_from_current_project = "select_ifc_file_from_current_project";
        private const string bsAPICall_put_file_in_current_project = "put_file_in_current_project";

        private const string bsAPICall_select_associated_file_from_current_project = "select_associated_file_from_current_project";
        private const string bsAPICall_put_associated_file_in_current_project = "put_associated_file_in_current_project";

        private const string bsAPICall_get_file_path_in_current_project = "get_file_path_in_current_project";
        private const string bsAPICall_exists_updated_file_version_current_project = "exists_updated_file_version_current_project";

        private const string bsAPIResponse_TRUE = "YES";
        private const string bsAPIResponse_FALSE = "NO";
        private const string bsAPIResponse_ERROR = "ERROR";
        private const string bsAPIResponse_BLOCKED_RESOURCE = "BLOCKED_RESOURCE";

        #endregion

        #region API configuration
        public static void setDebugMode(bool debugMode)
        {
            bsDebugMode = debugMode;
        }

        private static string getApiExecutablePath()
        {
            string apiExecutablePath;
            RegistryKey localMachineKey64bits = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = localMachineKey64bits.OpenSubKey(@"SOFTWARE\BIMserver.center\Synchronizer");

            if (key == null)
            {
                apiExecutablePath = null;
            }
            else
            {
                apiExecutablePath = key.GetValue(@"InstallDir").ToString();
                key.Close();
            }

            return apiExecutablePath;
        }

        /// <summary>
        /// Looks for the path to the API in the Windows Registry
        /// </summary>
        public static bool isApiInstalled()
        {
            string ApiExecutablePath = getApiExecutablePath();
            return (ApiExecutablePath != null);
        }

        /// <summary>
        /// Sets the BIMserver.center database path.
        /// 
        /// BIMserver.center api requires a folder where it will store its information and write back the api call results. 
        /// This folder must be associated to your model file (If you implement a "Save as" method in your software, it must also copy this folder.
        /// </summary>
        /// <param name="database_path"></param>
        public static void setDatabasePath(string database_path)
        {
            bsDataBaseFolder = database_path;
        }

        /// <summary>
        /// Sets the BIMserver.center login path.
        /// 
        /// BIMserver.center api requires a folder where it will store its information and write back the api call results. 
        /// The login API methods doesn't require a database and maybe you will need to check the login status before having a model loaded.
        /// </summary>
        /// <param name="login_path"></param>
        public static void setLoginPath(string login_path)
        {
            bsLoginFolder = login_path;
        }

        /// <summary>
        /// Sets the output language for BIMserver.center dialog's and api responses.
        /// </summary>
        /// <param name="language"></param>
        public static void setLanguage(BIMserverAPILanguage language)
        {
            switch (language)
            {
                case BIMserverAPILanguage.SPANISH:   bsLanguage = "ES"; break;
                case BIMserverAPILanguage.ENGLISH:   bsLanguage = "EN"; break;
                case BIMserverAPILanguage.FRENCH:    bsLanguage = "FR"; break;
                case BIMserverAPILanguage.ITALIAN:   bsLanguage = "IT"; break;
                case BIMserverAPILanguage.DEUSTCH:   bsLanguage = "DE"; break;

                default:        bsLanguage = "EN"; break;
            }
        }
        #endregion

        #region Database
        /// <summary>
        /// Creates a new database in the previous specified database path.
        /// Returns BIMserverAPIResponseType.TRUE if correct, BIMserverAPIResponseType.ERROR otherwise.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse createDatabase()
        {
            string arguments = makeAPICall(bsAPICall_create_database);
            return executeAPICallAndGetStatus(arguments, bsAPICall_create_database, bsDataBaseFolder);
        }
        #endregion

        #region Login
        /// <summary>
        /// Returns if the user is logged
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse isLogged()
        {
            string arguments = makeAPICall(bsAPICall_is_logged);
            return executeAPICallAndGetStatus(arguments, bsAPICall_is_logged, bsLoginFolder);
        }

        /// <summary>
        /// Shows the BIMserver.center provided login form. 
        /// 
        /// Returns BIMserverAPIResponseType.TRUE is user is logged, BIMserverAPIResponseType.FALSE otherwise. 
        /// BIMserverAPIResponseType.ERROR can occur. If more than one forms is opened using the same database, the next ones will get
        /// a BLOCKED_RESOURCE response.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse loginForm()
        {
            string arguments = makeAPICall(bsAPICall_login);
            return executeAPICallAndGetStatus(arguments, bsAPICall_login, bsLoginFolder);
        }

        /// <summary>
        /// Performs a connection with the given username and password (text plain).
        /// 
        /// Returns BIMserverAPIResponseType.TRUE is user is logged, BIMserverAPIResponseType.FALSE otherwise. 
        /// BIMserverAPIResponseType.ERROR can occur. If more than one forms is opened using the same database, the next ones will get
        /// a BLOCKED_RESOURCE response.
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public static BIMserverAPIResponse connect(string userName, string userPassword)
        {
            string arguments = makeAPICall(bsAPICall_connect) + " " + userName + " " + userPassword;
            return executeAPICallAndGetStatus(arguments, bsAPICall_connect, bsLoginFolder);
        }

        /// <summary>
        /// Disconnects the current user.
        /// 
        /// Returns BIMserverAPIResponseType.TRUE is user is logged, BIMserverAPIResponseType.ERROR otherwise. 
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse disconnect()
        {
            string arguments = makeAPICall(bsAPICall_disconnect);
            return executeAPICallAndGetStatus(arguments, bsAPICall_disconnect, bsLoginFolder);
        }

        /// <summary>
        /// Instructs BIMserver.center platform to send a recovery password e-mail to the given direction.
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public static BIMserverAPIResponse sendUserRecoverPassword(string userEmail)
        {
            string arguments = makeAPICall(bsAPICall_send_user_recover_password_email) + " " + userEmail;
            return executeAPICallAndGetStatus(arguments, bsAPICall_send_user_recover_password_email, bsLoginFolder);
        }

        /// <summary>
        /// Gets the current user name connected to the platform.
        /// 
        /// Returns BIMserverAPIResponseType.TRUE with the name in the variable text if correct,
        /// BIMserverAPIResponseType.ERROR otherwise.
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form or show the user logged data.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse getLoggedUserName()
        {
            string arguments = makeAPICall(bsAPICall_get_logged_user_name);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_name, bsLoginFolder);
        }

        /// <summary>
        /// Gets the current user e-mail connected to the platform.
        /// 
        /// Returns BIMserverAPIResponseType.TRUE with the e-mail in the variable text if correct,
        /// BIMserverAPIResponseType.ERROR otherwise.
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form or show the user logged data.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse getLoggedUserEmail()
        {
            string arguments = makeAPICall(bsAPICall_get_logged_user_email);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_email, bsLoginFolder);
        }

        /// <summary>
        /// Gets the current user image connected to the platform.
        /// 
        /// Returns BIMserverAPIResponseType.TRUE with the absolute path to the image if correct,
        /// BIMserverAPIResponseType.ERROR otherwise.
        /// 
        /// Utility method provided for implementing your own BIMserver.center login form or show the user logged data.
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse getLoggedUserImage()
        {
            string arguments = makeAPICall(bsAPICall_get_logged_user_image);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_get_logged_user_image, bsLoginFolder);
        }

        #endregion

        #region Current project management
        /// <summary>
        /// Shows the current project selection dialog.
        /// 
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is user pressed OK, BIMserverAPIResponseType.FALSE if not. 
        /// BIMserverAPIResponseType.ERROR can occur. If more than one forms is opened using the same database, the next ones will get
        /// a BLOCKED_RESOURCE response.
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse selectCurrentProject()
        {
            string arguments = makeAPICall(bsAPICall_select_current_project);
            return executeAPICallAndGetStatus(arguments, bsAPICall_select_current_project, bsDataBaseFolder);
        }

        /// <summary>
        /// Shows the ifc selection dialog.
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is user pressed OK and the project relative path of the selected file at variable filePathRelativeToProject.
        /// BIMserverAPIResponseType.FALSE if user cancelled. 
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse selectIfcFileFromCurrectProject()
        {
            string arguments = makeAPICall(bsAPICall_select_ifc_file_from_current_project);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_select_ifc_file_from_current_project, bsDataBaseFolder);
        }

        private static string stringEnclosedInQuotes(string text)
        { 
            return "\"" + text + "\"";
        }

        /// <summary>
        /// Shows the associated file selection dialog.
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is user pressed OK and the project relative path of the selected file at variable filePathRelativeToProject.
        /// BIMserverAPIResponseType.FALSE if user cancelled. 
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        public static BIMserverAPIResponse selectAssociatedFileFromCurrectProject(string extensionFilter)
        {
            string arguments = makeAPICall(bsAPICall_select_associated_file_from_current_project) + " " + getFilterByExtensionParameter(extensionFilter);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_select_associated_file_from_current_project, bsDataBaseFolder);
        }

        /// <summary>
        /// Adds a file to the BIMserver.center project
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is file was added.
        /// BIMserverAPIResponseType.ERROR if any problem arised. 
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse putFileInCurrentProject(string absoluteFilePath)
        {
            string arguments = makeAPICall(bsAPICall_put_file_in_current_project) + " " + stringEnclosedInQuotes(absoluteFilePath);
            return executeAPICallAndGetStatus(arguments, bsAPICall_put_file_in_current_project, bsDataBaseFolder);
        }

        /// <summary>
        /// Adds a associeted file to the BIMserver.center project
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is file was added.
        /// BIMserverAPIResponseType.ERROR if any problem arised. 
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse putAssociatedFileInCurrentProject(
                        string absoluteFilePath,
                        string file_description,
                        string app_name, string app_version, string app_description)

        {
            string arguments = makeAPICall(bsAPICall_put_associated_file_in_current_project) + " " + stringEnclosedInQuotes(absoluteFilePath);

            if (file_description != null && file_description.Length > 0)
                arguments += " -file_desc " + " " + stringEnclosedInQuotes(file_description);

            if (app_name != null && app_name.Length > 0)
                arguments += " -app_name " + " " + stringEnclosedInQuotes(app_name);

            if (app_version != null && app_version.Length > 0)
                arguments += " -app_version " + " " + stringEnclosedInQuotes(app_version);

            if (app_description != null && app_description.Length > 0)
                arguments += " -app_description " + " " + stringEnclosedInQuotes(app_description);

            return executeAPICallAndGetStatus(arguments, bsAPICall_put_associated_file_in_current_project, bsDataBaseFolder);
        }

        /// <summary>
        /// Gets the absolute path for the current project.
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE is project was setted beforehand, absolute path at variable absolutePath of the response.
        /// BIMserverAPIResponseType.ERROR if any problem arised. 
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse getAbsoluteCurrentProjectPath()
        {
            string arguments = makeAPICall(bsAPICall_get_current_project_path);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_get_current_project_path, bsDataBaseFolder);
        }

        /// <summary>
        /// Gets the absolute path for the the given relative project file path.
        /// 
        /// Use parameter updateDatabaseTimeStamp to instruct the API to update the last file access register.
        /// 
        /// Returns:
        /// 
        /// BIMserverAPIResponseType.TRUE. The absolute path at variable absolutePath of the response.
        /// BIMserverAPIResponseType.ERROR if any problem arised. 
        /// 
        /// </summary>
        /// <returns></returns>
        public static BIMserverAPIResponse convertRelativeProjectFilePathToAbsolute(string filePathRelativeToProject, bool updateDatabaseTimeStamp)
        {
            string arguments = makeAPICall(bsAPICall_get_file_path_in_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject) + " " + getUpdateDatabaseTimeStamp(updateDatabaseTimeStamp);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_get_file_path_in_current_project, bsDataBaseFolder);
        }

        /// <summary>
        /// Checks if exists an updated version of given file, identified by its relative to project file path.
        /// </summary>
        /// <param name="filePathRelativeToProject"></param>
        /// <returns></returns>
        public static BIMserverAPIResponse existsUpdatedFileVersion(string filePathRelativeToProject)
        {
            string arguments = makeAPICall(bsAPICall_exists_updated_file_version_current_project) + " " + stringEnclosedInQuotes(filePathRelativeToProject);
            return executeAPICallAndGetResponseText(arguments, bsAPICall_exists_updated_file_version_current_project, bsDataBaseFolder);
        }
        #endregion

        #region API invocation
        private static BIMserverAPIResponse executeAPICallAndGetStatus(string arguments, string apiCall, string api_response_folder)
        {
            bool get_another_line_for_true_response;

            get_another_line_for_true_response = false;
            return executeAPICallAndProcessResponse(arguments, apiCall, api_response_folder, get_another_line_for_true_response);
        }

        private static BIMserverAPIResponse executeAPICallAndGetResponseText(string arguments, string apiCall, string api_response_folder)
        {
            bool get_another_line_for_true_response;

            get_another_line_for_true_response = true;
            return executeAPICallAndProcessResponse(arguments, apiCall, api_response_folder, get_another_line_for_true_response);
        }

        private static BIMserverAPIResponse executeAPICallAndProcessResponse(
                        string arguments,
                        string apiCall, 
                        string api_response_folder, 
                        bool get_another_line_for_true_response)
        {
            BIMserverAPIResponse response;

            try
            {
                executeApiCall(arguments, api_response_folder);

                string responseFileName = apiCallResponseFilePath(apiCall, api_response_folder);
                StreamReader fs = File.OpenText(responseFileName);

                string loggin_status = fs.ReadLine();

                switch (loggin_status)
                {
                    case bsAPIResponse_TRUE:
                    {
                        if (get_another_line_for_true_response == true)
                        {
                            string absoluteFilePath = fs.ReadLine();
                            response = BIMserverAPIResponse.makeTrueResponseWithResponseText(absoluteFilePath);
                        }
                        else
                        {
                            response = BIMserverAPIResponse.makeTrueResponse();
                        }
                        break;
                    }

                    case bsAPIResponse_FALSE:

                        response = BIMserverAPIResponse.makeFalseResponse();
                        break;

                    case bsAPIResponse_BLOCKED_RESOURCE:
                    {
                        string error_message;

                        error_message = fs.ReadLine();
                        response = BIMserverAPIResponse.makeBlockedResourceResponse(error_message);
                        break;
                    }

                    case bsAPIResponse_ERROR:
                    {
                        string error_message = fs.ReadLine();
                        response = BIMserverAPIResponse.makeErrorResponse(error_message);
                        break;
                    }

                    default:

                        response = BIMserverAPIResponse.makeErrorResponse("Unknown response type");
                        break;
                }

                fs.Close();
            }
            catch (Exception e)
            {
                response = BIMserverAPIResponse.makeErrorResponse(e.Message);
            }

            return response;
        }

        private static void executeApiCall(string api_parameters, string api_response_folder)
        {
            try
            {
                Process process;
                bool started;

                process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = getApiExecutablePath() + "\\" + bsApiExecutableName;

                process.StartInfo.Arguments = getLanguageParameter();
                process.StartInfo.Arguments += " " + getWorkingDirectoryParameter(api_response_folder);
                process.StartInfo.Arguments += " " + api_parameters;
                process.StartInfo.Arguments += " " + getDebugParameter();

                started = process.Start();
                Debug.Assert(started == true);

                process.WaitForExit();
            }
            catch (Win32Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static string getWorkingDirectoryParameter(string api_response_folder)
        {
            return "-wd " + stringEnclosedInQuotes(api_response_folder);
        }

        private static string getLanguageParameter()
        {
            return "-lang " + stringEnclosedInQuotes(bsLanguage);
        }

        private static string getFilterByExtensionParameter(string extensionFilter)
        {
            return "-filter_by_file_extension " + stringEnclosedInQuotes(extensionFilter);
        }

        private static string getDebugParameter()
        {
            if (bsDebugMode == true)
                return "-debug";
            else
                return "";
        }

        private static string makeAPICall(string api_call)
        {
            return "-" + api_call;
        }

        private static string getUpdateDatabaseTimeStamp(bool updateDatabaseTimeStamp)
        {
            if (updateDatabaseTimeStamp == false)
                return "-no_update_db_timestamp";
            else
                return "";
        }

        private static string apiCallResponseFilePath(string api_call, string api_response_folder)
        {
            return api_response_folder + "\\" + api_call + ".txt";
        }
        #endregion
    }
}