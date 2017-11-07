using UnityEngine;
using System.Collections;


// This is the AI Player manager.
// Unlike the first version of this tutorial, most of the heavy lifting is done for us in BoardManager.
public class AIPlayer : MonoBehaviour
{
		private BoardManager board;

		// Use this for initialization
		void Start ()
		{
	
				board = FindObjectOfType <BoardManager> ();
		
				if (board == null)
						Debug.Log ("ERROR: Could not find BoardManager script!");
	
		}


		// It's the AI's turn to play...
		public void PlayAIMove (pieces piece)
		{
				// Because we need to use a coroutine to fake a 'thinking' period, this is just a very simple shell function...
				
				// Note that we only care about the piece - whether we're "X" or "O". Nothing else is relevant.
				int move = board.GetBestMove ();
		
				StartCoroutine (DoMove (move, piece));	// Use a coroutine to actually play the move.

		}
		
		// This coroutine fakes some thinking time before actually playing the move.
		private IEnumerator DoMove (int move, pieces piece)
		{
				
				yield return new WaitForSeconds (1.5f);
	
				board.AIPlayMoveAt (move, piece);
			
		}
		
}
