﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using Org.BouncyCastle.Crypto.Engines;
using magic.node;
using magic.signals.contracts;
using magic.lambda.crypto.rsa;

namespace magic.lambda.crypto.slots.rsa
{
    /// <summary>
    /// [crypto.rsa.encrypt] slot to encrypt some content using a public key that can only be decrypted
    /// using its public key.
    /// </summary>
    [Slot(Name = "crypto.rsa.encrypt")]
    public class Encrypt : ISlot
    {
        /// <summary>
        /// Implementation of slot.
        /// </summary>
        /// <param name="signaler">Signaler invoking slot.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Encrypter.Encrypt(input, new RsaEngine());
        }
    }
}
