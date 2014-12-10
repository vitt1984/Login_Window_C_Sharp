/*
 * Created by SharpDevelop.
 * User: Vittorio
 * Date: 18/11/2014
 * Time: 20:25
 * 
 */
using System;
using System.Collections.Generic;

using System.Text;
using System.Security.Cryptography;

namespace Test2
{
	/// <summary>
    /// Model class, to store existing logins and handle authentication
	/// </summary>
	
	public class Model
	{
		// dict holding the username and hashed password pairs
		private Dictionary <string,string> _dict = new Dictionary<string,string>();
		
		// to hash the password
		SHA512 _shaM = new SHA512Managed();
			
		public Model()
		{
			// for the sake of testing at least one login
			_dict.Add("username", hashPassword("password"));
		}
		
		// Paremters: username and password strings
		// Returns true if all these conditions are true:
		// 1) username and password not null
		// 2) username exists
		// 3) password matches the existing hash
		// else false.
		
		public bool authenticate(string iUsername, string iPassword) 
		{
			return iUsername != null && iPassword != null && 
				   _dict.ContainsKey(iUsername) && verifyHash(iPassword, _dict[iUsername]);
		}
		
		// Parameters : password to hash
		// Return : hashed value with random salt
		
		public string hashPassword(string iPassword)
		{
			Random random = new Random();
			int aSaltSize = random.Next(4, 8);

			byte[] aSaltBytes = new byte[aSaltSize];

			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

			rng.GetNonZeroBytes(aSaltBytes);
			return hashPasswordWithSalt (iPassword,aSaltBytes);			
		}
		
		// Parameters : password to hash and salt bytes[] to hash with
		// Return : hashed value
		
		public string hashPasswordWithSalt(string iPassword,
                                           byte[] iSaltBytes)
		{
			
			// password + salt to byte[]
			byte[] aPasswordBytes = Encoding.UTF8.GetBytes(iPassword);
			byte[] aPasswordWithSaltBytes =
				new byte[aPasswordBytes.Length + iSaltBytes.Length];
			
			// append password and salt
			for (int i=0; i < aPasswordBytes.Length; i++)
				aPasswordWithSaltBytes[i] = aPasswordBytes[i];
			for (int i=0; i < iSaltBytes.Length; i++)
				aPasswordWithSaltBytes[aPasswordBytes.Length + i] = iSaltBytes[i];
				
	        byte[] aHashBytes = _shaM.ComputeHash(aPasswordWithSaltBytes);
	        
	        // append salt to hashed password
	        byte[] aHashWithSaltBytes = new byte[aHashBytes.Length + 
	                                            iSaltBytes.Length];
	        for (int i=0; i < aHashBytes.Length; i++)
	            aHashWithSaltBytes[i] = aHashBytes[i];
	        for (int i=0; i < iSaltBytes.Length; i++)
	            aHashWithSaltBytes[aHashBytes.Length + i] = iSaltBytes[i];
	            
	        return Convert.ToBase64String(aHashWithSaltBytes);			
		}
		
		// Parameters : password to verify and existing hash
		// Return : true if the hash of the password matches the existing one
		
		public bool verifyHash(string iPassword,
		                         string iStoredHash)
		{        
			byte[] aHashWithSaltBytes = Convert.FromBase64String(iStoredHash);
			
			const int aHashSizeInBytes = 512 / 8; // as it is SHA512 
	
	        if (aHashWithSaltBytes.Length < aHashSizeInBytes)
	            return false;
	
	        // extracting salt from stored hash
	        byte[] aSaltBytes = new byte[aHashWithSaltBytes.Length - 
	                                    aHashSizeInBytes];
	        for (int i=0; i < aSaltBytes.Length; i++)
	            aSaltBytes[i] = aHashWithSaltBytes[aHashSizeInBytes + i];
	
	        // hash password with extracted salt
	        string aExpectedHashString = 
	                    hashPasswordWithSalt(iPassword, aSaltBytes);
	        
	        // check if it matches
	        return (aExpectedHashString == iStoredHash);
			
		}
		
	}
	
	
}
