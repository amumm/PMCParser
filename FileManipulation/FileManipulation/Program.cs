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
            GeneratePreProc();
        }

        public static void MoveFiles()
        {
            Manager manager = new Manager(
                "D://UKBiobank//Neuroimages//result",
                "D://UKBiobank//Neuroimages//nifty//");
            manager.RenameAndRelocate();
        }

        public static void GeneratePreProc(){
            String parent = @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped";

            String parentSubjectDirectory =     @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Subjects";
            String jobFilesResultDirectory =    @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Jobs";
            String batchFilesResultDirectory =  @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Batch";

            String pathToSubjectFoldersOnServer =   @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Subjects";
            String pathToJobFilesOnServer =         @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Jobs";

            String dumbyJobFilePath =   @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Batch_RestingStatefMRI_Preproc(A,R,W,S)_job.m";
            String dumbyBatchFilePath = @"R:\FSHN\Willette_Research\ROS_MAP\Baseline\Raw\Resting_state\Unzipped\Batch_RestingStatefMRI_Preproc(A,R,W,S).m";

            PreProcCreator preProc = new PreProcCreator(parentSubjectDirectory, jobFilesResultDirectory);

            preProc.GeneratePreProcJobFiles(pathToSubjectFoldersOnServer, dumbyJobFilePath);
            preProc.GeneratePreProcBatchFiles(pathToJobFilesOnServer, batchFilesResultDirectory, dumbyBatchFilePath);
        }
    }
}
