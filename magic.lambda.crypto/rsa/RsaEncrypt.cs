﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.crypto.rsa
{
    /// <summary>
    /// [crypto.rsa.encrypt] slot to encrypt some content using a public key.
    /// </summary>
    [Slot(Name = "crypto.rsa.encrypt")]
    public class RsaEncrypt : ISlot
    {
        /// <summary>
        /// Implementation of slot.
        /// </summary>
        /// <param name="signaler">Signaler invoking slot.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving arguments.
            var message = Encoding.UTF8.GetBytes(input.GetEx<string>());
            var raw = input.Children.FirstOrDefault(x => x.Name == "raw")?.GetEx<bool>() ?? false;

            // Converting key from base64 encoded DER format.
            var publicKey = Helpers.GetPublicKey(input);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            encryptEngine.Init(true, publicKey);
            if (raw)
                input.Value = encryptEngine.ProcessBlock(message, 0, message.Length);
            else
                input.Value = Convert.ToBase64String(encryptEngine.ProcessBlock(message, 0, message.Length));
        }
    }
}
