using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using MediaPortal.GUI.Library;
using System.Windows.Forms;
using System.ServiceModel;
using System.Runtime.Serialization;
using SharpLib.Display;

//////////////////////////////////////////////////////////////////////////

namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers.SharpLibDisplay
{
  /// <summary>
  /// SoundGraph iMON MiniDisplay implementation.
  /// Provides access to iMON Display API.
  /// </summary>
  public class Display : BaseDisplay
  {
    Client iClient;
    DataField iBitmapField;
    DataField iTextFieldTop;
    DataField iTextFieldBottom;
    DataField[] iFields;
    bool iNeedUpdate;

    public Display()
    {
      Initialized = false;
      iNeedUpdate = true;
    }

    //
    void CreateFields()
    {

      if (SupportsGraphics)
      {
        //Create fields for testing our graphics support.
        //Currently not used in production environment
        iBitmapField = new DataField(0);
        iBitmapField.RowSpan = 2;
        iTextFieldTop = new DataField(1);
        iTextFieldBottom = new DataField(2);

        iFields = new DataField[] { iBitmapField, iTextFieldTop, iTextFieldBottom };
      }
      else
      {
        if (SharpLibDisplay.Settings.Instance.SingleLine)
        {
          //Just make sure both fields are instantiated
          //Top one won't be displayed though
          iTextFieldTop = new DataField(1);
          iTextFieldBottom = new DataField(0);
          //Single line mode, only use bottom field
          iFields = new DataField[] { iTextFieldBottom };
        }
        else
        {
          iTextFieldTop = new DataField(0);
          iTextFieldBottom = new DataField(1);
          //Two lines mode use both fields
          iFields = new DataField[] { iTextFieldTop, iTextFieldBottom };
        }
      }

      iClient.CreateFields(iFields);

    }

    //From IDisplay
    public override bool SupportsGraphics { get { return false; } }

    //From IDisplay
    public override bool SupportsText { get { return true; } }

    /// <summary>
    /// Tell whether or not our SoundGraph Display plug-in is initialized
    /// </summary>
    protected bool Initialized { get; set; }

    //From IDisplay
    public override string Description { get { return "SharpLibDisplay"; } }

    //From IDisplay
    //Notably used when testing to put on the screen
    public override string Name
    {
      get
      {
        return "SharpLibDisplay";
      }
    }

    //
    private bool CheckDisplay()
    {
      if (iClient == null || !iClient.IsReady())
      {
        //Attempt to recover
        //LogDebug("SoundGraphDisplay.CheckDisplay(): Trying to recover");
        CleanUp();
        Initialize();
        return false;
      }

      return true;
    }

    //From IDisplay
    public override void Update()
    {
      //Check our display connection and whether we need an update.
      if (CheckDisplay() && iNeedUpdate)
      {
        //Display connection should be good
        iClient.SetFields(iFields);
        iNeedUpdate = false;
      }
    }

    //
    protected string ClassErrorName { get; set; }

    protected string UnsupportedDeviceErrorMessage { get; set; }


    public override string ErrorMessage
    {
      get
      {
        if (IsDisabled)
        {
          return "ERROR: SharpDisplayManager";
        }
        return string.Empty;
      }
    }

    //From IDisplay

    public override bool IsDisabled
    {
      get
      {
        //To check if our display is enabled we need to initialize it.
        Initialize();
        bool res = !Initialized;
        CleanUp();
        return res;
      }
    }

    //From IDisplay
    public override void Dispose()
    {
      CleanUp();
    }

    //From IDisplay
    public override void Initialize()
    {
      try
      {
        iClient = new Client();
        iClient.Open();
        iClient.SetName("MediaPortal");

        Initialized = true;
        if (SupportsGraphics)
        {
          SetLayoutWithBitmap();
        }
        else
        {
          if (SharpLibDisplay.Settings.Instance.SingleLine)
          {
            SetLayoutWithSingleLine();
          }
          else
          { 
            SetLayoutWithTwoLines();
          }
        }
        //
        CreateFields();
      }
      catch (System.Exception ex)
      {
        Log.Error(
        "SharpDisplayManager.Display.Initialize(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
        new object[] { ex.StackTrace });

        //Rollback
        iClient = null;
        Initialized = false;
      }
    }

    //From IDisplay
    public override void CleanUp()
    {
      if (iClient != null)
      {
        try
        {
          iTextFieldTop.Text = "";
          iTextFieldBottom.Text = "";
          iClient.SetFields(iFields);
          iClient.Close();
        }
        catch (System.Exception ex)
        {
          Log.Error(
          "SharpDisplayManager.Display.CleanUp(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
          new object[] { ex.StackTrace });
        }

        iClient = null;
        Initialized = false;
      }
    }

    //From IDisplay
    public override void SetLine(int line, string message, ContentAlignment aAlignment)
    {
      CheckDisplay();

      if (!Initialized)
      {
        return;
      }

      //
      if (line == 0 && (iTextFieldTop.Text != message || iTextFieldTop.Alignment != aAlignment))
      {
        iNeedUpdate = true;
        iTextFieldTop.Text = message;
        iTextFieldTop.Alignment = aAlignment;
        //iClient.SetField(iTextFieldTop);
      }
      else if (line == 1 && (iTextFieldBottom.Text != message || iTextFieldBottom.Alignment != aAlignment))
      {
        iNeedUpdate = true;
        iTextFieldBottom.Text = message;
        iTextFieldBottom.Alignment = aAlignment;
        //iClient.SetField(iTextFieldBottom);
      }

    }

    //From IDisplay
    public override void Configure()
    {
      //We need to have an initialized display to be able to configure it
      Initialize();

      //Our display is initialized, now open the advanced setting dialog
      SharpLibDisplaySettingsForm form = new SharpLibDisplaySettingsForm();
      form.ShowDialog();
      form.Dispose();

      CleanUp();
    }

    //From IDisplay
    public override void DrawImage(Bitmap aBitmap)
    {
      //Set a bitmap for our first field            
      int x1 = 0;
      int y1 = 0;
      int x2 = 64;
      int y2 = 64;

      Bitmap bitmap = new Bitmap(x2, y2);
      Pen blackPen = new Pen(Color.Black, 3);

      // Draw line to screen.

      using (var graphics = Graphics.FromImage(bitmap))
      {
        graphics.DrawImage(aBitmap, new Rectangle(x1, y1, x2, y2));
        //graphics.DrawLine(blackPen, x1, y1, x2, y2);
        //graphics.DrawLine(blackPen, x1, y2, x2, y1);
      }

      //aBitmap.Save("D:\\mp.png");

      //Not sure why passing aBitmap directly causes an exception in SharpDisplayManager WFC layers
      iBitmapField.Bitmap = bitmap;

    }

    //From IDisplay
    public override void SetCustomCharacters(int[][] customCharacters)
    {
      // Not supported
    }

    //From IDisplay
    public override void Setup(string port,
        int lines, int cols, int delay,
        int linesG, int colsG, int timeG,
        bool backLight, int backLightLevel,
        bool contrast, int contrastLevel,
        bool BlankOnExit)
    {
      // iMON VFD/LCD cannot be setup
    }

    public void CloseConnection()
    {
      if (iClient != null)
      {
        try
        {
          iClient.Close();
        }
        catch (System.Exception ex)
        {
          Log.Error(
          "SharpDisplayManager.Display.CloseConnection(): CAUGHT EXCEPTION {0}\n\n{1}\n\n", ex.Message,
          new object[] { ex.StackTrace });
        }

        iClient = null;
        Initialized = false;
      }
    }

    /// <summary>
    /// Define a layout with a bitmap field on the left and two lines of text on the right.
    /// </summary>
    private void SetLayoutWithBitmap()
    {
      //Define a 2 by 2 layout
      TableLayout layout = new TableLayout(2, 2);
      //First column only takes 25%
      layout.Columns[0].Width = 25F;
      //Second column takes up 75% 
      layout.Columns[1].Width = 75F;
      //Send layout to server
      iClient.SetLayout(layout);
    }

    /// <summary>
    /// </summary>
    private void SetLayoutWithTwoLines()
    {
      //Define a 1 column by 2 rows layout
      TableLayout layout = new TableLayout(1, 2);
      //First column only takes 25%
      layout.Columns[0].Width = 100F;
      //Send layout to server
      iClient.SetLayout(layout);
    }

    /// <summary>
    /// </summary>
    private void SetLayoutWithSingleLine()
    {
      //Define a 1 column by 2 rows layout
      TableLayout layout = new TableLayout(1, 1);
      //First column only takes 25%
      layout.Columns[0].Width = 100F;
      //Send layout to server
      iClient.SetLayout(layout);
    }



  }
}

