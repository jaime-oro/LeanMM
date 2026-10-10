using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

class Program
{
    class ModelFileInfo
    {
        public string FILE_NAME { get; set; } = String.Empty;
        public string PHYS_NAME { get; set; } = String.Empty;
    }

    static async Task Main(string[] args)
    {
        string connectionString = @"Data Source=P520_JORO;Initial Catalog=WM_clone_29092026;Integrated Security=True;Trust Server Certificate=True;";
        string masterDataName = "30005075";

        // pruebas en remoto con cable @ 1Gbps
        //
        // ref                                   mm                          lmm                     ratio mejora 
        // 80296300 portico  (214 archivos)      3m    (0.84s/archivo)       31s (0.14s/archivo)     x  5.8        
        // 30005075 misurata (3541 archivos)     6h15m (6.354s/archivo)      24m (0.41s/archivo)     x 15.6       


        string sql = @"
            SELECT l.CHILD_ELEMENT
            INTO #ChildTree
            FROM medmgr.MASTERDATA m
            JOIN medmgr.WM_ELE_LINKS l ON m.INFOS = l.PARENT_ELEMENT
            WHERE m.NAME = @MasterDataName;

            WHILE @@ROWCOUNT > 0
            BEGIN
                INSERT INTO #ChildTree (CHILD_ELEMENT)
                SELECT DISTINCT l.CHILD_ELEMENT
                FROM medmgr.WM_ELE_LINKS l
                JOIN #ChildTree t ON l.PARENT_ELEMENT = t.CHILD_ELEMENT
                WHERE NOT EXISTS (
                    SELECT 1 FROM #ChildTree ex WHERE ex.CHILD_ELEMENT = l.CHILD_ELEMENT
                );
            END;

            WITH RankedModels AS (
                SELECT 
                    f.FILE_NAME,
                    f.PHYS_NAME,
                    ROW_NUMBER() OVER (PARTITION BY m3d.NAME ORDER BY m3d.VERSION DESC) AS rn
                FROM #ChildTree t
                JOIN medmgr.MODEL_3D m3d ON t.CHILD_ELEMENT = m3d.INFOS
                JOIN medmgr.WM_FILES f ON m3d.INFOS = f.ELID
            )
            SELECT DISTINCT 
                FILE_NAME, 
                PHYS_NAME
            FROM RankedModels
            WHERE rn = 1;";


        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        IEnumerable<ModelFileInfo> files;

        using (var db = new SqlConnection(connectionString))
        {
            db.Open();
            var parameters = new { MasterDataName = masterDataName };
            files = db.Query<ModelFileInfo>(sql, parameters);
            Console.WriteLine($"{System.Linq.Enumerable.Count(files)} archivos.\n");
        }

        await new Program().CopyFilesFastAsync(files);

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:F2} s");
        Console.ReadKey();
    }

    async Task CopyFilesFastAsync(IEnumerable<ModelFileInfo> files)
    {
        string ROOT_FOLDER = @"\\192.168.1.20\vault\Sql_Area_205\Area1\";
        string DEST_FOLDER = @"C:\Users\joro\Desktop\Nueva carpeta (3)";

        var fileList = files as ICollection<ModelFileInfo> ?? files.ToList();
        int totalFiles = fileList.Count;
        int completedFiles = 0;

        var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };

        await Parallel.ForEachAsync(fileList, options, async (file, token) =>
        {
            string destPath = Path.Combine(DEST_FOLDER, file.FILE_NAME);
            string sourcePath = Path.Combine(ROOT_FOLDER, file.PHYS_NAME[..6], file.PHYS_NAME[6..7], file.PHYS_NAME[7..8], file.PHYS_NAME[8..]);

            using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, FileOptions.Asynchronous);
            using var destStream = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 81920, FileOptions.Asynchronous);

            await sourceStream.CopyToAsync(destStream, token);

            int current = Interlocked.Increment(ref completedFiles);
            lock (Console.Out)
            {
                double percentage = (double)current / totalFiles * 100;
                Console.Write($"\rProgreso: {current}/{totalFiles} archivos copiados ({percentage:F1}%)  ");
            }
        });
    }
}