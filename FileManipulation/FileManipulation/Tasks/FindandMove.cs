using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileManipulation
{
    class Manager
    {

        private DirectoryInfo ParentDirectory;

        private String ResultPath;

        public Manager(String path, String resultPath)
        {
            ParentDirectory = new DirectoryInfo(path);
            ResultPath = resultPath;
        }

        public void RenameAndRelocate()
        {
            DirectoryInfo[] set1, set2, set3;
            FileInfo[] files;
            String newName;

            set1 = ParentDirectory.GetDirectories();
            Console.WriteLine(set1.Length);
            GrantAccess(ParentDirectory.FullName);
            foreach (var subjectDirectory in set1) //all subject folders
            {
                set2 = subjectDirectory.GetDirectories();
                newName = subjectDirectory.Name.Substring(0, 7) + "_vol_bl.nii.gz";
                FileInfo newFile = new FileInfo(ResultPath + newName);

                foreach (var t1Directory in set2) // all folders in subject folder
                {
                    set3 = t1Directory.GetDirectories();

                    foreach (var t1FastDirectory in set3) // all folders in T1 folder
                    {
                        if (t1FastDirectory.Name.Contains("unusable"))
                        {
                            subjectDirectory.MoveTo("D://UKBiobank//Neuroimages//unusable//" + subjectDirectory.Name);
                            Console.WriteLine(subjectDirectory.Name);
                            break;
                        }
                        files = t1FastDirectory.GetFiles();

                        foreach (var file in files) // all files in T1_fast folder
                        {
                            if (file.Name.Contains("T1_brain_pve_1") && newFile == null)
                            {
                                file.CopyTo(ResultPath + newName);
                                Console.WriteLine(newName);
                            }
                        }

                    }
                }
            }
        }

        

        private static void GrantAccess(string file)
        {
            bool exists = System.IO.Directory.Exists(file);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
                Console.WriteLine("The Folder is created Sucessfully");
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
            DirectoryInfo dInfo = new DirectoryInfo(file);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }
    }
}
