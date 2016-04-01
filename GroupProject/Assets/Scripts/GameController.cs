using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    int dealersFirstCard = -1;

    public CardStack player;
    public CardStack Player2;
    public CardStack dealer;
    public CardStack deck;

    public InputField Input;
    public Text CurrentBet;
    public Text Player1Liqudity;
    public Text Player2Liquidity;

    public Button hitButton;
    public Button stickButton;
    public Button PlayAgainButton;

    public Text WinnerText;
    /*
     * Cards dealt to each player
     * First player hits/sticks/bust
     * Dealer's turn; must have minimum of 17 score hand
     * Dealers cards; first card is hidden, subsequent cards are facing
     */

    #region Public methods

    public void Hit()
    {
        player.Push(deck.Pop());
        if (player.HandValue() > 21)
        {
            // TODO: The player is bust
            hitButton.interactable = false;
            stickButton.interactable = false;
        }
    }

    public void Stick()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
//        StartCoroutine(AiAlgoritham());       line below must be deleted and added in when the algoritham has finished it's turn 
        StartCoroutine(DealersTurn());
    }
    public void PlayAgain()
    {

        PlayAgainButton.interactable = false;
        dealersFirstCard = -1;
        player.GetComponent<CardStackView>().Clear();
        Player2.GetComponent<CardStackView>().Clear();
        dealer.GetComponent<CardStackView>().Clear();
        deck.GetComponent<CardStackView>().Clear();
        deck.CreateDeck();
        hitButton.interactable = true;
        stickButton.interactable = true;

        StartGame();
    }
    public void Updatebet()
    {


        int PlayerLiquid = 0;
        int x = 0;
        int y = 0;
        int.TryParse(Player1Liqudity.text, out PlayerLiquid);
        int.TryParse(Input.text, out x);
        int.TryParse(CurrentBet.text, out y);
        if (x < PlayerLiquid)
        {
            PlayerLiquid -= x;
            x = x * 2;
            y = y + x;

            CurrentBet.text = y.ToString();
            Player1Liqudity.text = PlayerLiquid.ToString();
        }
        else
        {
            Input.text = "Bet too large.";
        }

    }
    #endregion

    #region Unity messages

    void Start()
    {
        StartGame();
    }

    #endregion

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            Player2.Push(deck.Pop());
            HitDealer();
        }
    }

    void HitDealer()
    {
        int card = deck.Pop();

        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
        }
    }

    IEnumerator DealersTurn()
    {
        int y = 0;
        int x = 0;
        int z = 0;
        int i = 0;
        hitButton.interactable = false;
        stickButton.interactable = false;
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        yield return new WaitForSeconds(1f);

        while (dealer.HandValue() < 17)
        {
            HitDealer();
            yield return new WaitForSeconds(1f);
        }
        if (player.HandValue() > 21 && Player2.HandValue() > 21 || dealer.HandValue() >= player.HandValue() && dealer.HandValue() >= Player2.HandValue() && dealer.HandValue() <= 21)
        {
            WinnerText.text = "Dealer Wins!";
            CurrentBet.text = "0";

        }
        else if (dealer.HandValue() > 21 && player.HandValue() <= 21 && Player2.HandValue() <= 21)
        {
            WinnerText.text = "both players win, pot is split!";
            int.TryParse(CurrentBet.text, out y);
            int.TryParse(Player1Liqudity.text, out z);
            int.TryParse(Player2Liquidity.text, out x);
            y = y / 2;
            z += y;
            x += y;
            Player1Liqudity.text = z.ToString();
            Player2Liquidity.text = x.ToString();
        }
        else if (player.HandValue() >= Player2.HandValue() && player.HandValue() <= 21)
        {
            WinnerText.text = "player 1 Wins!";
            int.TryParse(CurrentBet.text, out y);
            int.TryParse(Player1Liqudity.text, out z);
            z += y;
            Player1Liqudity.text = z.ToString();
        }
        else if (Player2.HandValue() >= player.HandValue() && Player2.HandValue() <= 21)
        {
            WinnerText.text = "player 2 Wins!";
            int.TryParse(CurrentBet.text, out y);
            int.TryParse(Player2Liquidity.text, out x);
            x += y;
            Player2Liquidity.text = x.ToString();
        }
        PlayAgainButton.interactable = true;
    }
}
