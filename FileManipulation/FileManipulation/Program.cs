using FileManipulation.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            //MoveFiles();
            //GeneratePreProc();
        }

        public static void MoveFiles()
        {
            Manager manager = new Manager(
                "D://UKBiobank//Neuroimages//result",
                "D://UKBiobank//Neuroimages//nifty//");
            manager.RenameAndRelocate();
        }

        public static void GeneratePreProc(){
            String parentSubjectDirectory = @"D:\UKBiobank\Neuroimages\result";
            String jobFilesResultDirectory = @"D:\Batch_and_Job\Jobs";
            String pathToSubjectFoldersOnServer = @"R:\FSHN\Willette_Research\UkBiobank\Neuroimages\Resting_state_FMRI\Unzipped\Subjects";
            String pathToJobFilesOnServer = @"R:\FSHN\Willette_Research\UkBiobank\Neuroimages\Resting_state_FMRI\Unzipped\Jobs";
            String batchFilesResultDirectory = @"D:\Batch_and_Job\Batch";

            PreProcCreator preProc = new PreProcCreator(parentSubjectDirectory, jobFilesResultDirectory);
            preProc.GeneratePreProcJobFiles(pathToSubjectFoldersOnServer);
            preProc.GeneratePreProcBatchFiles(pathToJobFilesOnServer, batchFilesResultDirectory);
        }
    }
}
