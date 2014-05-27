using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Automation.App_Code
{
    public class BrowserOptions
    {
        public enum BrowserOption : uint
        {
            /// <summary>
            /// No flags are set.
            /// </summary>
            None = 0,
            /// <summary>
            /// The browser will operate in offline mode. Equivalent to DLCTL_FORCEOFFLINE.
            /// </summary>
            AlwaysOffline = 0x10000000,
            /// <summary>
            /// The browser will play background sounds. Equivalent to DLCTL_BGSOUNDS.
            /// </summary>
            BackgroundSounds = 0x00000040,
            /// <summary>
            /// Specifies that the browser will not run Active-X controls. Use this setting
            /// to disable Flash movies. Equivalent to DLCTL_NO_RUNACTIVEXCTLS.
            /// </summary>
            DontRunActiveX = 0x00000200,
            /// <summary>
            ///  Specifies that the browser should fetch the content from the server. If the server's
            ///  content is the same as the cache, the cache is used.Equivalent to DLCTL_RESYNCHRONIZE.
            /// </summary>
            IgnoreCache = 0x00002000,
            /// <summary>
            ///  The browser will force the request from the server, and ignore the proxy, even if the
            ///  proxy indicates the content is up to date.Equivalent to DLCTL_PRAGMA_NO_CACHE.
            /// </summary>
            IgnoreProxy = 0x00004000,
            /// <summary>
            ///  Specifies that the browser should download and display images. This is set by default.
            ///  Equivalent to DLCTL_DLIMAGES.
            /// </summary>
            Images = 0x00000010,
            /// <summary>
            ///  Disables downloading and installing of Active-X controls.Equivalent to DLCTL_NO_DLACTIVEXCTLS.
            /// </summary>
            NoActiveXDownload = 0x00000400,
            /// <summary>
            ///  Disables web behaviours.Equivalent to DLCTL_NO_BEHAVIORS.
            /// </summary>
            NoBehaviours = 0x00008000,
            /// <summary>
            ///  The browser suppresses any HTML charset specified.Equivalent to DLCTL_NO_METACHARSET.
            /// </summary>
            NoCharSets = 0x00010000,
            /// <summary>
            ///  Indicates the browser will ignore client pulls.Equivalent to DLCTL_NO_CLIENTPULL.
            /// </summary>
            NoClientPull = 0x20000000,
            /// <summary>
            ///  The browser will not download or display Java applets.Equivalent to DLCTL_NO_JAVA.
            /// </summary>
            NoJava = 0x00000100,
            /// <summary>
            ///  The browser will download framesets and parse them, but will not download the frames
            ///  contained inside those framesets.Equivalent to DLCTL_NO_FRAMEDOWNLOAD.
            /// </summary>
            NoFrameDownload = 0x00080000,
            /// <summary>
            ///  The browser will not execute any scripts.Equivalent to DLCTL_NO_SCRIPTS.
            /// </summary>
            NoScripts = 0x00000080,
            /// <summary>
            ///  If the browser cannot detect any internet connection, this causes it to default to
            ///  offline mode.Equivalent to DLCTL_OFFLINEIFNOTCONNECTED.
            /// </summary>
            OfflineIfNotConnected = 0x80000000,
            /// <summary>
            ///  Specifies that UTF8 should be used.Equivalent to DLCTL_URL_ENCODING_ENABLE_UTF8.
            /// </summary>
            UTF8 = 0x00040000,
            /// <summary>
            ///  The browser will download and display video media.Equivalent to DLCTL_VIDEOS.
            /// </summary>
            Videos = 0x00000020
        }


        #region COM Interfaces
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }




        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }



        [ComVisible(true), Guid("0000011B-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleContainer
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] Object pbc,
                [In, MarshalAs(UnmanagedType.LPWStr)] String pszDisplayName, [Out,
                MarshalAs(UnmanagedType.LPArray)] int[] pchEaten, [Out,
                MarshalAs(UnmanagedType.LPArray)] Object[] ppmkOut);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnumObjects([In, MarshalAs(UnmanagedType.U4)] uint grfFlags, [Out,
                MarshalAs(UnmanagedType.LPArray)] Object[] ppenum);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int LockContainer([In, MarshalAs(UnmanagedType.Bool)] Boolean fLock);
        }

        [ComVisible(true), Guid("00000118-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleClientSite
        {
            [PreserveSig]
            int SaveObject();
            [PreserveSig]
            int GetMoniker([In, MarshalAs(UnmanagedType.U4)] int dwAssign, [In, MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);
            [PreserveSig]
            int GetContainer(out object container);
            [PreserveSig]
            int ShowObject();
            [PreserveSig]
            int OnShowWindow(int fShow);
            [PreserveSig]
            int RequestNewObjectLayout();
        }

        [ComVisible(true), ComImport(),
        Guid("00000112-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IOleObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetClientSite([In, MarshalAs(UnmanagedType.Interface)] IOleClientSite
                pClientSite);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetClientSite([Out, MarshalAs(UnmanagedType.Interface)] out IOleClientSite site);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetHostNames([In, MarshalAs(UnmanagedType.LPWStr)] String
                szContainerApp, [In, MarshalAs(UnmanagedType.LPWStr)] String
                szContainerObj);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Close([In, MarshalAs(UnmanagedType.U4)] uint dwSaveOption);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetMoniker([In, MarshalAs(UnmanagedType.U4)] uint dwWhichMoniker, [In,
                MarshalAs(UnmanagedType.Interface)] Object pmk);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetMoniker([In, MarshalAs(UnmanagedType.U4)] uint dwAssign, [In,
                MarshalAs(UnmanagedType.U4)] uint dwWhichMoniker, [Out, MarshalAs(UnmanagedType.Interface)] out Object moniker);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int InitFromData([In, MarshalAs(UnmanagedType.Interface)] Object
                pDataObject, [In, MarshalAs(UnmanagedType.Bool)] Boolean fCreation, [In,
                MarshalAs(UnmanagedType.U4)] uint dwReserved);
            int GetClipboardData([In, MarshalAs(UnmanagedType.U4)] uint dwReserved, out
			Object data);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int DoVerb([In, MarshalAs(UnmanagedType.I4)] int iVerb, [In] IntPtr lpmsg,
                [In, MarshalAs(UnmanagedType.Interface)] IOleClientSite pActiveSite, [In,
                MarshalAs(UnmanagedType.I4)] int lindex, [In] IntPtr hwndParent, [In] RECT
                lprcPosRect);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnumVerbs(out Object e); // IEnumOLEVERB
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OleUpdate();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsUpToDate();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetUserClassID([In, Out] ref Guid pClsid);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetUserType([In, MarshalAs(UnmanagedType.U4)] uint dwFormOfType, [Out,
                MarshalAs(UnmanagedType.LPWStr)] out String userType);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetExtent([In, MarshalAs(UnmanagedType.U4)] uint dwDrawAspect, [In]
			Object pSizel); // tagSIZEL
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetExtent([In, MarshalAs(UnmanagedType.U4)] uint dwDrawAspect, [Out]
			Object pSizel); // tagSIZEL
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Advise([In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IAdviseSink pAdvSink, out
			int cookie);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Unadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnumAdvise(out Object e);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetMiscStatus([In, MarshalAs(UnmanagedType.U4)] uint dwAspect, out int
                misc);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetColorScheme([In] Object pLogpal); // tagLOGPALETTE
        }


        [ComImport, Guid("B196B288-BAB4-101A-B69C-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleControl
        {
            [PreserveSig]
            int GetControlInfo([Out] object pCI);
            [PreserveSig]
            int OnMnemonic([In] ref MSG pMsg);
            [PreserveSig]
            int OnAmbientPropertyChange(int dispID);
            [PreserveSig]
            int FreezeEvents(int bFreeze);
        }

        #endregion COM Interfaces
    }
}
