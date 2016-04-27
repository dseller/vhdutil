/* MIT License

Copyright (c) 2016 Dennis Seller

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace vhdutil
{
    class Program
    {
        public const int ErrorSuccess = 0;
        public const int OpenVirtualDiskRwDepthDefault = 1;

        public const int VirtualStorageTypeDeviceVhd = 2;

        public static readonly Guid VirtualStorageTypeVendorMicrosoft = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 AttachVirtualDisk(IntPtr handle, IntPtr security, AttachVirtualDiskFlag flags,
            Int32 providerSpecificFlags, ref AttachVirtualDiskParameters parameters, IntPtr overlapped);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 DetachVirtualDisk(IntPtr handle, DetachVirtualDiskFlag flags, UInt64 flagsEx);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean CloseHandle(IntPtr hObject);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 OpenVirtualDisk(ref VirtualStorageType virtualStorageType, string path, VirtualDiskAccessMask virtualDiskAccessMask, OpenVirtualDiskFlag flags, ref OpenVirtualDiskParameters parameters, ref IntPtr handle);

        public static void Main(string[] args)
        {
            // vhdutil attach <file>
            // vhdutil detach <file>

            if (args.Length < 2)
                return;

            string cmd = args[0];

            try
            {
                if (cmd == "attach")
                {
                    IntPtr handle = IntPtr.Zero;

                    // open disk handle
                    var openParameters = new OpenVirtualDiskParameters();
                    openParameters.Version = OpenVirtualDiskVersion.OpenVirtualDiskVersion1;
                    openParameters.Version1.RWDepth = OpenVirtualDiskRwDepthDefault;

                    var openStorageType = new VirtualStorageType();
                    openStorageType.DeviceId = VirtualStorageTypeDeviceVhd;
                    openStorageType.VendorId = VirtualStorageTypeVendorMicrosoft;

                    int openResult = OpenVirtualDisk(ref openStorageType, args[1],
                        VirtualDiskAccessMask.VirtualDiskAccessAll, OpenVirtualDiskFlag.OpenVirtualDiskFlagNone,
                        ref openParameters, ref handle);
                    if (openResult != ErrorSuccess)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Native error {0}.", openResult));
                    }

                    // attach disk – permanently
                    var attachParameters = new AttachVirtualDiskParameters();
                    attachParameters.Version = AttachVirtualDiskVersion.AttachVirtualDiskVersion1;
                    int attachResult = AttachVirtualDisk(handle, IntPtr.Zero,
                        AttachVirtualDiskFlag.AttachVirtualDiskFlagPermanentLifetime, 0, ref attachParameters,
                        IntPtr.Zero);
                    if (attachResult != ErrorSuccess)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Native error {0}.", attachResult));
                    }

                    // close handle to disk
                    CloseHandle(handle);
                }
                else if (cmd == "detach")
                {
                    IntPtr handle = IntPtr.Zero;

                    // open disk handle
                    var openParameters = new OpenVirtualDiskParameters();
                    openParameters.Version = OpenVirtualDiskVersion.OpenVirtualDiskVersion1;
                    openParameters.Version1.RWDepth = OpenVirtualDiskRwDepthDefault;

                    var openStorageType = new VirtualStorageType();
                    openStorageType.DeviceId = VirtualStorageTypeDeviceVhd;
                    openStorageType.VendorId = VirtualStorageTypeVendorMicrosoft;

                    int openResult = OpenVirtualDisk(ref openStorageType, args[1],
                        VirtualDiskAccessMask.VirtualDiskAccessAll, OpenVirtualDiskFlag.OpenVirtualDiskFlagNone,
                        ref openParameters, ref handle);
                    if (openResult != ErrorSuccess)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Native error {0}.", openResult));
                    }

                    int attachResult = DetachVirtualDisk(handle, DetachVirtualDiskFlag.DetachVirtualDiskFlagNone, 0);
                    if (attachResult != ErrorSuccess)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                            "Native error {0}.", attachResult));
                    }

                    // close handle to disk
                    CloseHandle(handle);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: {0}\n{1}", e.Message, e.StackTrace);
            }
        }
    }
}
