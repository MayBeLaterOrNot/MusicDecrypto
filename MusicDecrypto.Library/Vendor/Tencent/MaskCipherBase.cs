﻿using System;
using System.Numerics;
using MusicDecrypto.Library.Numerics;

namespace MusicDecrypto.Library.Vendor.Tencent
{
    public abstract class MaskCipherBase : IStreamCipher
    {
        public void Decrypt(MarshalMemoryStream buffer)
        {
            var data = buffer.AsSimdPaddedSpan();
            var step = Simd.LaneCount;
            var mask = (stackalloc byte[step]);
            for (int i = 0; i < data.Length; i += step)
            {
                var window = data[i..(i + step)];
                GetMask(mask, i);
                var v = new Vector<byte>(window);
                var m = new Vector<byte>(mask);
                (m ^ v).CopyTo(window);
            }
        }

        protected abstract void GetMask(Span<byte> buffer, long indexOffset);
    }
}
