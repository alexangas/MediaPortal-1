using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MediaPortal.Configuration;

namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers.SharpLibDisplay
{

  [Serializable]
  public class Settings
  {
    public Settings()
    {
      //Init EqData
      MiniDisplayHelper.InitEQ(ref iEq);
    }

    public EQControl iEq;

    //SharpLibDisplay settings
    [XmlAttribute]
    public bool DisableWhenInBackground { get; set; }

    [XmlAttribute]
    public bool DisableWhenIdle { get; set; }

    [XmlAttribute]
    public int DisableWhenIdleDelayInSeconds { get; set; }

    [XmlAttribute]
    public bool ReenableWhenIdleAfter { get; set; }

    [XmlAttribute]
    public int ReenableWhenIdleAfterDelayInSeconds { get; set; }

    [XmlAttribute]
    public bool DisableWhenPlaying { get; set; }

    [XmlAttribute]
    public int DisableWhenPlayingDelayInSeconds { get; set; }

    [XmlAttribute]
    public bool ReenableWhenPlayingAfter { get; set; }

    [XmlAttribute]
    public int ReenableWhenPlayingAfterDelayInSeconds { get; set; }


    //Layout properties
    [XmlAttribute]
    public bool SingleLine { get; set; }

    //EQ Properties
    [XmlAttribute]
    public bool EqStartDelay { get; set; }

    [XmlAttribute]
    public int DelayEqTime { get; set; }


    [XmlAttribute]
    public int EqRate { get; set; }

    [XmlAttribute]
    public bool EqDisplay { get; set; }

    //Tells whether we periodically disable our EQ during play
    [XmlAttribute]
    public bool EqPeriodic { get; set; }

    //The time our EQ is disabled for
    [XmlAttribute]
    public int EqDisabledTimeInSeconds { get; set; }

    //The time our EQ is enabled for
    [XmlAttribute]
    public int EqEnabledTimeInSeconds { get; set; }

    [XmlAttribute]
    public bool RestrictEQ { get; set; }

    [XmlAttribute]
    public bool SmoothEQ { get; set; }
    //


    #region Delegates

    public delegate void OnSettingsChangedHandler();

    #endregion

    private static Settings m_Instance;
    public const string m_Filename = "MiniDisplay_SharpLibDisplay.xml";

    public static Settings Instance
    {
      get
      {
        if (m_Instance == null)
        {
          m_Instance = Load();
        }
        return m_Instance;
      }
      set { m_Instance = value; }
    }



    public static event OnSettingsChangedHandler OnSettingsChanged;

    //Load default settings in the given instance
    private static void Default(Settings aSettings)
    {
      aSettings.EqDisplay = false;
      aSettings.RestrictEQ = false;
      aSettings.EqRate = 10;
      aSettings.EqStartDelay = false;
      aSettings.DelayEqTime = 10;
      aSettings.SmoothEQ = false;

      //Layout properties
      aSettings.SingleLine = false;
      
    }

    public static Settings Load()
    {
      Settings settings;
      SoundGraphDisplay.LogDebug("SharpLibDisplay.Settings.Load(): started");
      if (File.Exists(Config.GetFile(Config.Dir.Config, m_Filename)))
      {
        SoundGraphDisplay.LogDebug("SharpLibDisplay.Settings.Load(): Loading settings from XML file");
        var serializer = new XmlSerializer(typeof(Settings));
        var xmlReader = new XmlTextReader(Config.GetFile(Config.Dir.Config, m_Filename));
        settings = (Settings)serializer.Deserialize(xmlReader);
        xmlReader.Close();
      }
      else
      {
        SoundGraphDisplay.LogDebug("SharpLibDisplay.Settings.Load(): Loading settings from defaults");
        settings = new Settings();
        Default(settings);
        SoundGraphDisplay.LogDebug("SharpLibDisplay.Settings.Load(): Loaded settings from defaults");
      }

      //Sync our EQ settings
      settings.iEq.UseEqDisplay = settings.EqDisplay;
      settings.iEq.DelayEQ = settings.EqStartDelay;
      settings.iEq._DelayEQTime = settings.DelayEqTime;
      settings.iEq.EQTitleDisplay = settings.EqPeriodic;
      settings.iEq._EQTitleShowTime = settings.EqDisabledTimeInSeconds;
      settings.iEq._EQTitleDisplayTime = settings.EqEnabledTimeInSeconds;


      SoundGraphDisplay.LogDebug("SharpLibDisplay.Settings.Load(): completed");
      return settings;
    }

    public static void NotifyDriver()
    {
      if (OnSettingsChanged != null)
      {
        OnSettingsChanged();
      }
    }

    public static void Save()
    {
      Save(Instance);
    }

    public static void Save(Settings ToSave)
    {
      var serializer = new XmlSerializer(typeof(Settings));
      var writer = new XmlTextWriter(Config.GetFile(Config.Dir.Config, m_Filename),
                                     Encoding.UTF8) { Formatting = Formatting.Indented, Indentation = 2 };
      serializer.Serialize(writer, ToSave);
      writer.Close();
    }

    public static void SetDefaults()
    {
      Default(Instance);
    }
  }
}
