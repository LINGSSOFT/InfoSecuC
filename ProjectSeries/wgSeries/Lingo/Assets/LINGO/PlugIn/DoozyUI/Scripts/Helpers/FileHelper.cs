// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DoozyUI
{
    public static class FileHelper
    {
        #region Validation

        public static bool fileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void CreateDirectoryIfDoesntExist(string filePath)
        {
#if (UNITY_WEBPLAYER == false)
            FileInfo file = new FileInfo(filePath);
            file.Directory.Create();
#endif
        }

        #endregion

        public static void writeObjectToFile<T>(string filePath, T obj, Action<FileStream, T> serializedMethod)
        {
            CreateDirectoryIfDoesntExist(filePath);

            FileStream stream = new FileStream(filePath, FileMode.Create);
            serializedMethod(stream, obj);
            stream.Close();
        }

        public static T readObjectFile<T>(string filename, Func<FileStream, T> deserializationMethod)
        {
            if (!fileExists(filename))
            {
                Debug.Log("ERROR: Can't load " + filename + " - no file exists");
            }

            FileStream stream = new FileStream(filename, FileMode.Open);
            T data = deserializationMethod(stream);
            stream.Close();

            return data;
        }

        public static void deleteObjectFile(string filePath)
        {
            File.Delete(filePath);
        }

        #region Serialization/Deserialization Methods

        public static void SerializeXML<T>(FileStream stream, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        public static T DeserializeXML<T>(FileStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        #endregion

        #region Misc Options

        public static FileInfo[] GetAllTheFilesFromFolder(string directoryPath)
        {
#if (UNITY_WEBPLAYER == false)
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            FileInfo[] fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            return fileInfo;
#else
        return null;
#endif
        }

        #endregion

        #region Get Folder Path
        /// <summary>
        /// Searches for the folderName in all the project's directories and returns the absolute path of the first one it encounters
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetFolderPath(string folderName)
        {
            string[] folderPath = Directory.GetDirectories(Application.dataPath, folderName, SearchOption.AllDirectories);

            if (folderPath == null)
            {
                Debug.LogError("You searched for the [" + folderName + "] folder, but there is no folder with that name in this project.");
                return "ERROR";
            }
            else if (folderPath.Length > 1)
            {
                Debug.LogWarning("You searched for the [" + folderName + "] folder and there are at least 2 folders with the same name in this project. Returned the folder location for the first one, but it might not be the one you're looking for so please give the folder you are looking for an unique name in the project.");
                return folderPath[0];
            }
            return folderPath[0];
        }

        /// <summary>
        /// Searches for the folderName in all the project's directories and returns the relative path of the first one it encounters
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetRelativeFolderPath(string folderName)
        {
            string folderPath = GetFolderPath(folderName);

            folderPath = folderPath.Replace(Application.dataPath, "Assets");

            return folderPath;
        }
        #endregion

    }
}
