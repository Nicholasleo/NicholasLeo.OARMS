//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 16:49:36
//    说明：
//    版权所有：个人
//***********************************************************
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace OARMS.Core.Infrastructure
{
    public interface IOarmsFileProvider : IFileProvider
    {
        /// <summary>
        /// 将字符串数组拼接到一个字符串中
        /// </summary>
        /// <param name="paths">字符串数组</param>
        /// <returns></returns>
        string Combine(params string[] paths);

        /// <summary>
        /// 通过路径创建不存在的目录和子目录
        /// </summary>
        /// <param name="path">路径</param>
        void CreateDirectory(string path);

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">文件全名</param>
        void CreateFile(string filePath);

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        void DeleteDirectory(string path);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        void DeleteFile(string filePath);

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="sourceDirName">源目录路径</param>
        /// <param name="destDirName">目标目录路径</param>
        void DirectoryMove(string sourceDirName, string destDirName);

        /// <summary>
        /// 获取指定路径中的搜索到可匹配的文件名并可以搜索子目录的枚举类集合，
        /// </summary>
        /// <param name="directoryPath">需要搜索的目录路径</param>
        /// <param name="searchPattern">匹配路径的检索字符串，可以包含有效文本路径通配符和文本路径（*and?）的组合，但是不支持正则表达式</param>
        /// <param name="topDirectoryOnly">指定搜索当前目录还是搜索当前目录和所有的子目录</param>
        /// <returns></returns>
        IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool topDirectoryOnly = true);

        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        /// <param name="overwrite">是否覆盖</param>
        void FileCopy(string sourceFileName, string destFileName, bool overwrite = false);

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        bool FileExists(string filePath);

        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        long FileLength(string filePath);

        /// <summary>
        /// 文件移动
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        void FileMove(string sourceFileName, string destFileName);

        /// <summary>
        /// 获取目录绝对路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        string GetAbsolutePath(params string[] paths);

        /// <summary>
        /// 获取用于封装指定目录的访问控制列表（ACL）项的System.Security.AccessControl.DirectorySecurity对象
        /// </summary>
        /// <param name="path">包含描述文件访问控制列表（ACL）信息的System.Security.AccessControl.DirectorySecurity对象的目录路径</param>
        /// <returns></returns>
        DirectorySecurity GetAccessControl(string path);

        /// <summary>
        /// 获取文件或目录创建时间
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DateTime GetCreationTime(string path);

        /// <summary>
        /// 获取指定目录中指定搜索模式匹配的子目录（包括其路径）的名称
        /// </summary>
        /// <param name="path">用于检索的目录路径</param>
        /// <param name="searchPattern">检索目录的名称匹配的搜索字符串，可以包含通配符但不支持正则表达式</param>
        /// <param name="topDirectoryOnly">指定是搜索当前目录，还是搜索当前目录和所有子目录</param>
        /// <returns></returns>
        string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// 获取指定路径的目录信息
        /// </summary>
        /// <param name="path">指定文件或目录路径</param>
        /// <returns>
        /// 路径的目录信息，如果路径为空或为根路径，则为空。
        /// 如果路径不包含目录信息，则返回System.String.Empty
        /// </returns>
        string GetDirectoryName(string path);

        /// <summary>
        /// 只返回指定路径的目录名
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>目录名</returns>
        string GetDirectoryNameOnly(string path);

        /// <summary>
        /// 获取路径指定的文件拓展名
        /// </summary>
        /// <param name="filePath">获取拓展名的路径</param>
        /// <returns>拓展名（包含“.”）</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        /// 获取路径中的文件名和拓展名
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件名称</returns>
        string GetFileName(string path);

        /// <summary>
        /// 获取不带拓展名的文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>不含拓展名的文件名</returns>
        string GetFileNameWithoutExtension(string filePath);

        /// <summary>
        /// 获取指定目录中指定搜索模式匹配的文件名（包括其路径）并指定是否搜索子目录。
        /// </summary>
        /// <param name="directoryPath">检索路径</param>
        /// <param name="searchPattern">
        /// 检索目录的名称匹配的搜索字符串，可以包含有效文本路径和通配符（*and？）的组合但不支持正则表达式
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 指定目录中文件的全名（包括路径）数组与指定的搜索模式匹配，如果找不到文件，则为空数组。
        /// </returns>
        string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// 获取上次访问指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">文件或目录。</param>
        /// <returns>文件的日期和时间的System.DateTime结构</returns>
        DateTime GetLastAccessTime(string path);

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the date and time that the specified file or directory was last written to.
        /// This value is expressed in local time
        /// </returns>
        DateTime GetLastWriteTime(string path);

        /// <summary>
        /// 获取指定文件或目录的最近更新时间
        /// </summary>
        /// <param name="path">文件或目录</param>
        /// <returns>
        /// 文件或目录上次写入的日期和时间的System.DateTime结构。该值以UTC时间表示
        /// </returns>
        DateTime GetLastWriteTimeUtc(string path);

        /// <summary>
        /// 获取指定路径的父目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>父目录，如果路径是根目录，则为空，包括UNC服务器或共享名的根目录。</returns>
        string GetParentDirectory(string directoryPath);

        /// <summary>
        /// 判断是都为目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>目录True，否则False</returns>
        bool IsDirectory(string path);

        /// <summary>
        /// 将虚拟路径映射到物理磁盘路径
        /// </summary>
        /// <param name="path">虚拟路径，例如. "~/bin"</param>
        /// <returns>物理路径，例如. "c:\inetpub\wwwroot\bin"</returns>
        string MapPath(string path);

        /// <summary>
        /// 将文件内容读取到字节数组中
        /// </summary>
        /// <param name="filePath">读取的文件</param>
        /// <returns>
        /// 文件内容的字节数组
        /// </returns>
        byte[] ReadAllBytes(string filePath);

        /// <summary>
        /// 打开文件，用指定的编码读取文件的所有行，然后关闭文件
        /// </summary>
        /// <param name="path">读取文件路径</param>
        /// <param name="encoding">指定文件编码</param>
        /// <returns>文件所有行的字符串</returns>
        string ReadAllText(string path, Encoding encoding);

        /// <summary>
        /// 设置文件最新更新时间
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lastWriteTimeUtc"></param>
        /// <return></return>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <summary>
        /// 将字节写入文件中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bytes"></param>
        void WriteAllBytes(string filePath, byte[] bytes);

        /// <summary>
        /// 使用指定编码将字符串写入文件中，并关闭文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        /// <param name="encoding"></param>
        void WriteAllText(string path, string contents, Encoding encoding);
    }
}
