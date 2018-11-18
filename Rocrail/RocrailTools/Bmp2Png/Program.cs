/*
 * MIT License
 *
 * Copyright (c) 2018 Dr. Christian Benjamin Ries
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bmp2Png
{
	class Program
	{
		static void Main(string[] args)
		{
			var infile = args[0];
			if (!File.Exists(infile))
			{
				Console.WriteLine("File does not exist: {0}", infile);
			}

			string targetName = "";

			try
			{
				Bitmap img = new Bitmap(infile);
				var dirname = Path.GetDirectoryName(infile);
				var fname = Path.GetFileNameWithoutExtension(infile);
				targetName = Path.Combine(dirname, fname + ".png");
				if (File.Exists(targetName))
					File.Delete(targetName);

				img.Save(targetName, ImageFormat.Png);

				var finfo = new FileInfo(targetName);
				if (!finfo.Exists)
				{
					Console.WriteLine("<error> File not created: {0}", targetName);
				}
				else
				{
					Console.WriteLine("File created: {0}", targetName);
					Console.WriteLine("Size: {0} Bytes", finfo.Length);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Target: {0}", targetName);
				Console.WriteLine("<error> {0}", ex.Message);
			}
		}
	}
}
