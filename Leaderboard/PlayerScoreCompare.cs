// PlayerScoreCompare : Descrption : Work in association with LeaderboardSystem.cs to compare all the score
using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class PlayerScoreCompare : IComparable<PlayerScoreCompare>
{
	public string name;
	public string score;
	public string _loop;
	public int total;

	public PlayerScoreCompare (string newName ,string newScore,string new_loop,int newtotal)
	{
		name = newName;
		score = newScore;
		_loop = new_loop;
		total = newtotal;
	}

	//This method is required by the IComparable
	//interface. 
	public int CompareTo(PlayerScoreCompare other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return the difference in power.
		return total - other.total;
	}
}