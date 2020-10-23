﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Text;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.crypto.utilities;

namespace magic.lambda.crypto
{
    /// <summary>
    /// [crypto.decrypt] slot that decrypts and verifies the
    /// specified content using the specified arguments.
    /// </summary>
    [Slot(Name = "crypto.decrypt")]
    public class Decrypt : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving arguments.
            var content = Utilities.GetContent(input, true);
            var decryptionKey = Utilities.GetKeyFromArguments(input, "decryption-key");
            var raw = input.Children.FirstOrDefault(x => x.Name == "raw")?.GetEx<bool>() ?? false;

            // House cleaning.
            input.Clear();
            input.Value = null;

            // Decrypting content and returning to caller.
            var decrypter = new Decrypter(decryptionKey);
            var result = decrypter.Decrypt(content);
            if (raw)
            {
                input.Value = result.Content;
                input.Add(new Node("signature", result.Signature));
            }
            else
            {
                input.Value = Encoding.UTF8.GetString(result.Content);
                input.Add(new Node("signature", Convert.ToBase64String(result.Signature)));
            }
            input.Add(new Node("fingerprint", result.Fingerprint));
        }
    }
}