using System;
using System.Collections.Generic;
using System.IO;

namespace DateFolderFromContent
{
	class Program
	{
		static void Main(string[] args)
		{
			ModifyDirs(Directory.GetCurrentDirectory());
		}

		private static void ModifyDirs(string baseDir)
		{
			Console.WriteLine("Modifying directories in " + baseDir);
			string[] dirs = Directory.GetDirectories(baseDir);
			Console.WriteLine("Processing " + dirs.Length + " directories...");
			int modified = 0;
			foreach (string dir in dirs)
			{
				try
				{
					if (ModifyDir(dir))
					{
						modified++;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to modify " + dir);
					Console.WriteLine(ex.GetType() + ": " + ex.Message);
				}
			}

			Console.WriteLine("Modified " + modified + " directories.");
		}

		private static bool ModifyDir(string dir)
		{
			bool modified = false;
			string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
			if (files.Length > 0)
			{
				List<FileInfo> fileInfos = GetAllFileInfos(files);
				FileInfo largestFile = GetLargestFileInfo(fileInfos);
				Directory.SetLastWriteTime(dir, largestFile.LastWriteTime);
				modified = true;
			}
			return modified;
		}

		private static List<FileInfo> GetAllFileInfos(string[] files)
		{
			List<FileInfo> fileInfos = new List<FileInfo>(files.Length);
			foreach (string file in files)
			{
				fileInfos.Add(new FileInfo(file));
			}
			return fileInfos;
		}

		private static FileInfo GetLargestFileInfo(List<FileInfo> fileInfos)
		{
			fileInfos.Sort((x, y) => y.Length.CompareTo(x.Length));
			return fileInfos[0];
		}
	}
}
