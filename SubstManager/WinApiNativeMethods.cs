using System.Runtime.InteropServices;
using System.Text;

namespace SubstManager
{
    /// <summary>
    /// WinApiのラッパークラス
    /// </summary>
    public static class WinApiNativeMethods
    {
        private const int DRIVE_ASSIGN = 0;
        private const int DRIVE_UNASSIGN = 2;

        /// <summary>
        /// ドライブに指定したフォルダを割り当てる。
        /// </summary>
        /// <param name="driveName"></param>
        /// <param name="folderName"></param>
        public static void AssignDrive( string driveName, string folderName ) => DefineDosDevice( DRIVE_ASSIGN, driveName, folderName );

        /// <summary>
        /// ドライブの割り当てを削除する。
        /// </summary>
        /// <param name="driveName"></param>
        /// <param name="folderName"></param>
        public static void UnassignDrive( string driveName ) => DefineDosDevice( DRIVE_UNASSIGN, driveName, null );

        /// <summary>
        /// ドライブの説明を取得する。
        /// 取得に失敗または説明がない場合はnullを返す。
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public static string GetDriveDescription( string driveName )
        {
            const int MAX_LENGTH = 300;
            var buffer = new StringBuilder( MAX_LENGTH );

            var success = QueryDosDevice( driveName, buffer, buffer.Capacity );
            if ( success == 0 ) return null;

            var description = buffer.ToString();
            return string.IsNullOrEmpty( description ) ? null : description;
        }

        /// <summary>
        /// アプリケーションで、MS-DOS デバイス名の定義、再定義、または削除を実行します。
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="lpDeviceName"></param>
        /// <param name="lpTargetPath"></param>
        /// <returns></returns>
        [DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
        private static extern bool DefineDosDevice( int dwFlags, string lpDeviceName, string lpTargetPath );

        /// <summary>
        /// アプリケーションで MS-DOS デバイス名に関する情報を取得できるようにします。
        /// </summary>
        /// <param name="lpDeviceName"></param>
        /// <param name="lpTargetPath"></param>
        /// <param name="ucchMax"></param>
        /// <returns></returns>
        [DllImport( "kernel32.dll", CharSet = CharSet.Unicode )]
        private static extern uint QueryDosDevice( string lpDeviceName, StringBuilder lpTargetPath, int ucchMax );
    }
}