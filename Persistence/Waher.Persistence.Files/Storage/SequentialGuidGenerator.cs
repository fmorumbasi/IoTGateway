﻿using System;
using System.Collections;
using System.Collections.Generic;
#if NETSTANDARD1_5
using System.Security.Cryptography;
#endif
using System.Text;
using System.Threading.Tasks;

namespace Waher.Persistence.Files.Storage
{
	/// <summary>
	/// Class that generates sequential, albeit cyclical, GUIDs for use in databases. They are not guaranteed to be
	/// globally unique. They are however unique to a very high degree of probability.
	/// </summary>
	public class SequentialGuidGenerator : IDisposable
	{
		private static DateTime reference = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

#if NETSTANDARD1_5
		private RandomNumberGenerator gen;
#else
		private Random gen;
#endif
		private byte[] processId;
		private byte[] machineNr;
		private byte[] random;
		private uint counter;
		private bool reverseOrder;

		/// <summary>
		/// Class that generates sequential, albeit cyclical, GUIDs for use in databases. They are not guaranteed to be
		/// globally unique. They are however unique to a very high degree of probability.
		/// </summary>
		public SequentialGuidGenerator()
		{
#if NETSTANDARD1_5
			StringBuilder sb = new StringBuilder();

			sb.Append(Environment.MachineName);

			foreach (DictionaryEntry Entry in Environment.GetEnvironmentVariables())
			{
				sb.Append(Entry.Key.ToString());
				sb.Append(Entry.Value.ToString());
			}

			using (SHA1 Sha1 = SHA1.Create())
			{
				string MachineData = sb.ToString();
				byte[] Hash = Sha1.ComputeHash(Encoding.UTF8.GetBytes(MachineData));
				this.machineNr = new byte[3];
				Array.Copy(Hash, 0, this.machineNr, 0, 3);
			}

			this.gen = RandomNumberGenerator.Create();
			this.processId = BitConverter.GetBytes((ushort)(System.Diagnostics.Process.GetCurrentProcess().Id));

			byte[] b = new byte[4];
			this.gen.GetBytes(b);
#else
			this.gen = new Random();

			this.machineNr = new byte[3];
			this.gen.NextBytes(this.machineNr);

			this.processId = BitConverter.GetBytes((ushort)(DateTime.Now.Ticks));

			byte[] b = new byte[4];
			this.gen.NextBytes(b);
#endif
			this.random = new byte[3];

			this.counter = BitConverter.ToUInt32(b, 0);
			this.reverseOrder = !BitConverter.IsLittleEndian;
		}

		/// <summary>
		/// GUIDs are generated by sets of 16 bytes, that are defined as follows:
		/// 
		/// Bytes 0-3:		Number of seconds, since 2001-01-01. (Will wrap 2136-02-07, 06:28:16)
		/// Bytes 4-7:		Counter, started at a random number.
		/// Bytes 8-9:		Process ID.
		/// Bytes 10-12:	Machine number.
		/// Bytes 13-15:	Random number.
		/// </summary>
		/// <returns>New GUID</returns>
		public Guid CreateGuid()
		{
			byte[] b = new byte[16];
			uint Ticks = (uint)(DateTime.Now.ToUniversalTime() - reference).TotalSeconds;
			byte[] Buf;

			lock (this.gen)
			{
#if NETSTANDARD1_5
				this.gen.GetBytes(this.random);
#else
				this.gen.NextBytes(this.random);
#endif

				Buf = BitConverter.GetBytes(Ticks);
				if (this.reverseOrder)
					Array.Reverse(Buf);
				Array.Copy(Buf, 0, b, 0, 4);

				Buf = BitConverter.GetBytes(this.counter++);
				if (this.reverseOrder)
					Array.Reverse(Buf);

				b[4] = Buf[2];
				b[5] = Buf[3];
				b[6] = Buf[0];
				b[7] = Buf[1];

				Array.Copy(this.processId, 0, b, 8, 2);
				Array.Copy(this.machineNr, 0, b, 10, 3);
				Array.Copy(this.random, 0, b, 13, 3);
			}

			return new Guid(b);
		}

		/// <summary>
		/// <see cref="IDisposable.Dispose"/>
		/// </summary>
		public void Dispose()
		{
			if (this.gen != null)
			{
#if NETSTANDARD1_5
				this.gen.Dispose();
#endif
				this.gen = null;
			}
		}
	}
}
