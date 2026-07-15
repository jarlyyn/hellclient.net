namespace Hellclient.World.Shared;

using Hellclient.World.Contexts;
using Hellclient.World.Presets;

//统一对外的标准API实现
public static class ScriptAPI
{
    public const int EOK = 0;                             //No error
    public const int EWorldOpen = 30001;                  //The world is already open
    public const int EWorldClosed = 30002;                //The world is closed, this action cannot be performed
    public const int ENoNameSpecified = 30003;            //No name has been specified where one is required
    public const int ECannotPlaySound = 30004;            //The sound file could not be played
    public const int ETriggerNotFound = 30005;            //The specified trigger name does not exist
    public const int ETriggerAlreadyExists = 30006;       //Attempt to add a trigger that already exists
    public const int ETriggerCannotBeEmpty = 30007;       //The trigger "match" string cannot be empty
    public const int EInvalidObjectLabel = 30008;         //The name of this object is invalid
    public const int EScriptNameNotLocated = 30009;       //Script name is not in the script file
    public const int EAliasNotFound = 30010;              //The specified alias name does not exist
    public const int EAliasAlreadyExists = 30011;         //Attempt to add a alias that already exists
    public const int EAliasCannotBeEmpty = 30012;         //The alias "match" string cannot be empty
    public const int ECouldNotOpenFile = 30013;           //Unable to open requested file
    public const int ELogFileNotOpen = 30014;             //Log file was not open
    public const int ELogFileAlreadyOpen = 30015;         //Log file was already open
    public const int ELogFileBadWrite = 30016;            //Bad write to log file
    public const int ETimerNotFound = 30017;              //The specified timer name does not exist
    public const int ETimerAlreadyExists = 30018;         //Attempt to add a timer that already exists
    public const int EVariableNotFound = 30019;           //Attempt to delete a variable that does not exist
    public const int ECommandNotEmpty = 30020;            //Attempt to use SetCommand with a non-empty command window
    public const int EBadRegularExpression = 30021;       //Bad regular expression syntax
    public const int ETimeInvalid = 30022;                //Time given to AddTimer is invalid
    public const int EBadMapItem = 30023;                 //Direction given to AddToMapper is invalid
    public const int ENoMapItems = 30024;                 //No items in mapper
    public const int EUnknownOption = 30025;              //Option name not found
    public const int EOptionOutOfRange = 30026;           //New value for option is out of range
    public const int ETriggerSequenceOutOfRange = 30027;  //Trigger sequence value invalid
    public const int ETriggerSendToInvalid = 30028;       //Where to send trigger text to is invalid
    public const int ETriggerLabelNotSpecified = 30029;   //Trigger label not specified/invalid for 'send to variable'
    public const int EPluginFileNotFound = 30030;         //File name specified for plugin not found
    public const int EProblemsLoadingPlugin = 30031;      //There was a parsing or other problem loading the plugin
    public const int EPluginCannotSetOption = 30032;      //Plugin is not allowed to set this option
    public const int EPluginCannotGetOption = 30033;      //Plugin is not allowed to get this option
    public const int ENoSuchPlugin = 30034;               //Requested plugin is not installed
    public const int ENotAPlugin = 30035;                 //Only a plugin can do this
    public const int ENoSuchRoutine = 30036;              //Plugin does not support that subroutine (subroutine not in script)
    public const int EPluginCouldNotSaveState = 30037;    //Plugin could not save state (eg. no state directory)
    public const int EPluginDoesNotSaveState = 30038;     //Plugin does not support saving state
    public const int EPluginDisabled = 30039;             //Plugin is currently disabled
    public const int EErrorCallingPluginRoutine = 30040;  //Could not call plugin routine
    public const int ECommandsNestedTooDeeply = 30041;    //Calls to "Execute" nested too deeply
    public const int ECannotCreateChatSocket = 30042;     //Unable to create socket for chat connection
    public const int ECannotLookupDomainName = 30043;     //Unable to do DNS (domain name) lookup for chat connection
    public const int ENoChatConnections = 30044;          //No chat connections open
    public const int EChatPersonNotFound = 30045;         //Requested chat person not connected
    public const int EBadParameter = 30046;               //General problem with a parameter to a script call
    public const int EChatAlreadyListening = 30047;       //Already listening for incoming chats
    public const int EChatIDNotFound = 30048;             //Chat session with that ID not found
    public const int EChatAlreadyConnected = 30049;       //Already connected to that server/port
    public const int EClipboardEmpty = 30050;             //Cannot get (text from the) clipboard
    public const int EFileNotFound = 30051;               //Cannot open the specified file
    public const int EAlreadyTransferringFile = 30052;    //Already transferring a file
    public const int ENotTransferringFile = 30053;        //Not transferring a file
    public const int ENoSuchCommand = 30054;              //There is not a command of that name
    public const int EArrayAlreadyExists = 30055;         //That array already exists
    public const int EArrayDoesNotExist = 30056;          //That array does not exist
    public const int EBadKeyName = 30056;                //That name is not permitted for a key
    public const int EArrayNotEvenNumberOfValues = 30057; //Values to be imported into array are not in pairs
    public const int EImportedWithDuplicates = 30058;     //Import succeeded, however some values were overwritten
    public const int EBadDelimiter = 30059;               //Import/export delimiter must be a single character, other than backslash
    public const int ESetReplacingExistingValue = 30060;  //Array element set, existing value overwritten
    public const int EKeyDoesNotExist = 30061;            //Array key does not exist
    public const int ECannotImport = 30062;               //Cannot import because cannot find unused temporary character
    public const int EItemInUse = 30063;                  //Cannot delete trigger/alias/timer because it is executing a script
    public const int ESpellCheckNotActive = 30064;        //Spell checker is not active
    public const int ECannotAddFont = 30065;              //Cannot create requested font
    public const int EPenStyleNotValid = 30066;           //Invalid settings for pen parameter
    public const int EUnableToLoadImage = 30067;          //Bitmap image could not be loaded
    public const int EImageNotInstalled = 30068;          //Image has not been loaded into window
    public const int EInvalidNumberOfPoints = 30069;      //Number of points supplied is incorrect
    public const int EInvalidPoint = 30070;               //Point is not numeric
    public const int EHotspotPluginChanged = 30071;       //Hotspot processing must all be in same plugin
    public const int EHotspotNotInstalled = 30072;        //Hotspot has not been defined for this window
    public const int ENoSuchWindow = 30073;               //Requested miniwindow does not exist
    public const int EBrushStyleNotValid = 30074;         //Invalid settings for brush parameter
    public static string Version() => AppVersion.Version.FullVersionCode();
    public static void Note(IWorld world, string message) => world.DoPrint(message);
}