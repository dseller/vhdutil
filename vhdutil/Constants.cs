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
using System.Runtime.InteropServices;

namespace vhdutil
{
    public enum AttachVirtualDiskFlag
    {
        AttachVirtualDiskFlagNone = 0x00000000,
        AttachVirtualDiskFlagReadOnly = 0x00000001,
        AttachVirtualDiskFlagNoDriveLetter = 0x00000002,
        AttachVirtualDiskFlagPermanentLifetime = 0x00000004,
        AttachVirtualDiskFlagNoLocalHost = 0x00000008
    }

    public enum DetachVirtualDiskFlag
    {
        DetachVirtualDiskFlagNone = 0x00000000
    }

    public enum OpenVirtualDiskVersion
    {
        OpenVirtualDiskVersion1 = 1
    }

    public enum AttachVirtualDiskVersion
    {
        AttachVirtualDiskVersionUnspecified = 0,
        AttachVirtualDiskVersion1 = 1
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AttachVirtualDiskParametersVersion1
    {
        public Int32 Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AttachVirtualDiskParameters
    {
        public AttachVirtualDiskVersion Version;
        public AttachVirtualDiskParametersVersion1 Version1;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct VirtualStorageType
    {
        public Int32 DeviceId;
        public Guid VendorId;
    }

    public enum VirtualDiskAccessMask
    {
        VirtualDiskAccessAttachRo = 0x00010000,
        VirtualDiskAccessAttachRw = 0x00020000,
        VirtualDiskAccessDetach = 0x00040000,
        VirtualDiskAccessGetInfo = 0x00080000,
        VirtualDiskAccessCreate = 0x00100000,
        VirtualDiskAccessMetaops = 0x00200000,
        VirtualDiskAccessRead = 0x000d0000,
        VirtualDiskAccessAll = 0x003f0000,
        VirtualDiskAccessWritable = 0x00320000
    }

    public enum OpenVirtualDiskFlag
    {
        OpenVirtualDiskFlagNone = 0x00000000,
        OpenVirtualDiskFlagNoParents = 0x00000001,
        OpenVirtualDiskFlagBlankFile = 0x00000002,
        OpenVirtualDiskFlagBootDrive = 0x00000004
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OpenVirtualDiskParametersVersion1
    {
        public Int32 RWDepth;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OpenVirtualDiskParameters
    {
        public OpenVirtualDiskVersion Version;
        public OpenVirtualDiskParametersVersion1 Version1;
    }
}
