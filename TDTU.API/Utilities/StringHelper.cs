using System.Text;
using System.Text.RegularExpressions;

namespace TDTU.API.Utilities;

public static class StringHelper
{
	public static string convertToUnSign(string s)
	{
		try
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
		}
		catch (Exception ex)
		{
			return s;
		}
	}

	public static string BoDauVaKhoangTrang(string s)
	{
		s = s.ToLower();
		s = s.Replace(" ", "-");
		s = s.Replace(".", "dot");
		s = convertToUnSign(s);
		return s;
	}

	public static bool IsValidEmail(string email)
	{
		if (!string.IsNullOrEmpty(email))
		{
			return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

		}
		else
		{
			return false;
		}
	}

	public static string GenerateCode()
	{
		try
		{
			DateTime today = DateTime.Today;
			string datePart = today.ToString("ddMMyy");
			string randomString = GenerateRandomString(6).ToUpper();
			string finalCode = $"{randomString}-{datePart}";
			return finalCode;
		}
		catch (Exception ex)
		{
			return GenerateRandomString(6);
		}
	}

	public static string GenerateRandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		Random random = new Random();
		char[] result = new char[length];

		for (int i = 0; i < length; i++)
		{
			result[i] = chars[random.Next(chars.Length)];
		}

		return new string(result);
	}
}
