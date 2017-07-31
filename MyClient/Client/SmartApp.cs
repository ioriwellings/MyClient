using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartX1Demo
{
    public class SmartApp
    {
        bool _is64ibt = false;
        public SmartApp()
        {
            if (IntPtr.Size == 4)
                _is64ibt = true;
        }
        public int SmartX1Find(string appID, int[] keyHandles, int[] keyNumber)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1Find(appID, keyHandles, keyNumber);
            }
            else
            {
                return SmartX1_X64.SmartX1Find(appID, keyHandles, keyNumber);
            }
        }

        //open

        public int SmartX1Open(int keyHandle, int uPin1, int uPin2, int uPin3, int uPin4)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1Open(keyHandle, uPin1, uPin2, uPin3, uPin4);
            }
            else
            {
                return SmartX1_X64.SmartX1Open(keyHandle, uPin1, uPin2, uPin3, uPin4);
            }
        }
        //close


        public int SmartX1Close(int keyHandle)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1Close(keyHandle);
            }
            else
            {
                return SmartX1_X64.SmartX1Close(keyHandle);
            }
        }
        //checkExist


        public int SmartX1CheckExist(int keyHandle)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1CheckExist(keyHandle);
            }
            else
            {
                return SmartX1_X64.SmartX1CheckExist(keyHandle);
            }
        }
        //getUid

        public int SmartX1GetUid(int keyHandle, StringBuilder uid)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1GetUid(keyHandle, uid);
            }
            else
            {
                return SmartX1_X64.SmartX1GetUid(keyHandle, uid);
            }
        }
        //ReadStorage

        public int SmartX1ReadStorage(int keyHandle, int startAddr, int length, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1ReadStorage(keyHandle, startAddr, length, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1ReadStorage(keyHandle, startAddr, length, pBuffer);
            }
        }
        //WriteStorage

        public int SmartX1WriteStorage(int keyHandle, int startAddr, int length, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1WriteStorage(keyHandle, startAddr, length, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1WriteStorage(keyHandle, startAddr, length, pBuffer);
            }
        }
        //PageLogin

        public int SmartX1PageLogin(int keyHandle, int pageNo, byte[] password, int length)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1PageLogin(keyHandle, pageNo, password, length);
            }
            else
            {
                return SmartX1_X64.SmartX1PageLogin(keyHandle, pageNo, password, length);
            }
        }
        //PageLogout

        public int SmartX1PageLogout(int keyHandle, int pageNo)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1PageLogout(keyHandle, pageNo);
            }
            else
            {
                return SmartX1_X64.SmartX1PageLogout(keyHandle, pageNo);
            }
        }
        //ReadPage

        public int SmartX1ReadPage(int keyHandle, int pageNo, int startAddr, ref int length, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1ReadPage(keyHandle, pageNo, startAddr, ref length, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1ReadPage(keyHandle, pageNo, startAddr, ref length, pBuffer);
            }
        }
        //WritePage

        public int SmartX1WritePage(int keyHandle, int pageNo, int startAddr, int length, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1WritePage(keyHandle, pageNo, startAddr, length, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1WritePage(keyHandle, pageNo, startAddr, length, pBuffer);
            }
        }
        //ReadMem

        public int SmartX1ReadMem(int keyHandle, int start, int length, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1ReadMem(keyHandle, start, length, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1ReadMem(keyHandle, start, length, pBuffer);
            }
        }
        //WriteMem

        public int SmartX1WriteMem(int keyHandle, int start, int length, byte[] pBuffer)
        {
           if (_is64ibt)
           {
               return SmartX1_X86.SmartX1WriteMem( keyHandle,  start,  length,  pBuffer);
           }
           else
           {
               return SmartX1_X64.SmartX1WriteMem(keyHandle, start, length, pBuffer);
           }
       }
        //encrypt

        public int SmartX1TriDesEncrypt(int keyHandle, int buffSize, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1TriDesEncrypt(keyHandle, buffSize, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1TriDesEncrypt(keyHandle, buffSize, pBuffer);
            }
        }
        //desDecrypt

        public int SmartX1TriDesDecrypt(int keyHandle, int buffSize, byte[] pBuffer)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1TriDesDecrypt(keyHandle, buffSize, pBuffer);
            }
            else
            {
                return SmartX1_X64.SmartX1TriDesDecrypt(keyHandle, buffSize, pBuffer);
            }
        }
        //led

        public int SmartX1Led(int keyHandle, int state)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1Led(keyHandle, state);
            }
            else
            {
                return SmartX1_X64.SmartX1Led(keyHandle, state);
            }
        }

        public int SmartX1PageGetProperty(int keyHandle, int pageNo, int propId, int[] propValue)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1PageGetProperty(keyHandle, pageNo, propId, propValue);
            }
            else
            {
                return SmartX1_X64.SmartX1PageGetProperty(keyHandle, pageNo, propId, propValue);
            }
        }
        public int SmartX1GetSoftVersion(int keyHandle, int[] version)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.SmartX1GetSoftVersion(keyHandle, version);
            }
            else
            {
                return SmartX1_X64.SmartX1GetSoftVersion(keyHandle, version);
            }
        }

        public  string TridesEncrypt(string data, string key, Encoding encoding)
        {

            if (_is64ibt)
            {
                return SmartX1_X86.TridesEncrypt(data, key, encoding);
            }
            else
            {
                return SmartX1_X64.TridesEncrypt(data, key, encoding);
            }
        }

        public  string TridesDecEncrypt(byte[] data, string key, Encoding encoding)
        {
            if (_is64ibt)
            {
                return SmartX1_X86.TridesDecEncrypt(data, key, encoding);
            }
            else
            {
                return SmartX1_X64.TridesDecEncrypt(data, key, encoding);
            }
        }

    }
}
