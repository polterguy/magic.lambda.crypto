﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using magic.node;
using magic.node.extensions;
using magic.lambda.crypto.utilities;

namespace magic.lambda.crypto.rsa
{
    /*
     * Utility class to provide common functions for other classes and methods.
     */
    internal class Verifier
    {
        readonly AsymmetricKeyParameter _key;
        
        public Verifier(byte[] key)
        {
            _key = PublicKeyFactory.CreateKey(key);
        }

        /*
         * Verifies a cryptographic signature, according to caller's specifications.
         */
        internal void Verify(string algo, byte[] message, byte[] signature)
        {
            // Creating our signer and associating it with the private key.
            var signer = SignerUtilities.GetSigner($"{algo}withRSA");
            signer.Init(false, _key);

            // Signing the specified data, and returning to caller as base64.
            signer.BlockUpdate(message, 0, message.Length);
            if (!signer.VerifySignature(signature))
                throw new ArgumentException("Signature mismatch");
        }
    }
}
