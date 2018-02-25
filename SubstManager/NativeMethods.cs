using System.Runtime.InteropServices;
using System.Text;

namespace SubstManager
{
    /// <summary>
    /// WinApiを使えるようにするためのクラス
    /// </summary>
    public static class NativeMethods
    {
        public const int DRIVE_ASSIGN = 0;
        public const int DRIVE_UNASSIGN = 2;

        /// <summary>
        /// アプリケーションで、MS-DOS デバイス名の定義、再定義、または削除を実行します。
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="lpDeviceName"></param>
        /// <param name="lpTargetPath"></param>
        /// <returns></returns>
        [DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
        public static extern bool DefineDosDevice( int dwFlags, string lpDeviceName, string lpTargetPath );

        /// <summary>
        /// アプリケーションで MS-DOS デバイス名に関する情報を取得できるようにします。
        /// </summary>
        /// <param name="lpDeviceName"></param>
        /// <param name="lpTargetPath"></param>
        /// <param name="ucchMax"></param>
        /// <returns></returns>
        [DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
        public static extern uint QueryDosDevice( string lpDeviceName, StringBuilder lpTargetPath, int ucchMax );
    }
}