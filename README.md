## How is web automation work?

Web automation is a web browser which can help automation the browser like fill text, click on button, extract the data, save file from web automatically by javascript only.

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/HBultD9tnTw/0.jpg)](https://www.youtube.com/watch?v=HBultD9tnTw)

## Example

1. Login to Gmail

  - Open gmail: go('gmail.com');
  - Wait 5 seconds: sleep(5, false);
  - Fill username: fill("Email", "test");
  - Wait 1 second: sleep(1, false);
  - Fill password: fill("Passwd", "test");
  - Wait 1 second: sleep(1, false);
  - Click button Sign In: click("signIn");
  - Waith 2 seconds: sleep(2, false);
  - Get error message: var text = extract("errormsg_0_Passwd", "text");
  - Show error: alert(text);

2. Take website image

  //Take Snapshot
  - Location to save image: var location = getCurrentPath() + '\\image.png';
  - Save to image: takesnapshot(location);

![](https://github.com/thangdc/web-automation/raw/master/web-automation.png)

## Which thirdparty was used in the Web Automation project?

1. [GeckoFx](https://bitbucket.org/geckofx)
  - Replace web browser control
  - Geckofx-Core.dll
  - Geckofx-Winforms.dll
  - xulrunner (folder)
2. [Scintilla.NET](https://scintillanet.codeplex.com/)
  - Using for texteditor
  - SciLexer.dll
  - SciLexer64.dll
  - ScintillaNET.dll
3. [NPOI](https://npoi.codeplex.com/)
  - Working with excel file (xls)
  - NPOI.dll
4. [MouseKeyboardLibrary](http://www.codeproject.com/Articles/28064/Global-Mouse-and-Keyboard-Library)
  - Record your mouse and keyboard
  - MouseKeyboardLibrary.dll

## How to run Web Automation project?

- Make sure all thirdparty exists in output folder
- Click WebAutomation.sln to open project
- Because all dll above was inorge when upload to github so please get it from links above.
- [More Infomation](http://www.codeproject.com/Tips/525426/Web-Automation)

