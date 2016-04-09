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
    public Text Player2Cash;
    public int Player1Con = 5;
    public int Player2Con = 5;
    public int DealerCon = 5;
    public int hitValue = 0;

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
            Input.interactable = false;
            StartCoroutine(AITurn());
        }

    }

    public void Stick()
    {
        Input.interactable = false;
        hitButton.interactable = false;
        stickButton.interactable = false;
        StartCoroutine(AITurn());       //line below must be deleted and added in when the algoritham has finished it's turn 
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
        Input.interactable = true;
        WinnerText.text = " ";
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
            Input.text = "Thanks for the money";
            AIBet();
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
        if (player.HandValue() > 21 && Player2.HandValue() > 21)
        {
            WinnerText.text = "Dealer Wins!";
            CurrentBet.text = "0";
            DealerCon++;
            Player1Con--;
            Player2Con--;
        }
        else
        {
            while (dealer.HandValue() < 17)
            {
                HitDealer();
                yield return new WaitForSeconds(1f);
            }
            if (player.HandValue() > 21 && Player2.HandValue() > 21 && dealer.HandValue() <= 21 || dealer.HandValue() >= player.HandValue() && dealer.HandValue() >= Player2.HandValue() && dealer.HandValue() <= 21)
            {
                WinnerText.text = "Dealer Wins!";
                CurrentBet.text = "0";
                DealerCon++;
                Player1Con--;
                Player2Con--;
            }
            else if (dealer.HandValue() > 21 && player.HandValue() <= 21 && Player2.HandValue() <= 21)
            {
                WinnerText.text = "both players win, pot is split!";
                int.TryParse(CurrentBet.text, out y);
                int.TryParse(Player1Liqudity.text, out z);
                int.TryParse(Player2Cash.text, out x);
                y = y / 2;
                z += y;
                x += y;
                Player1Liqudity.text = z.ToString();
                Player2Cash.text = x.ToString();
                CurrentBet.text = "0";
                Player1Con++;
                Player2Con++;
                DealerCon--;
            }
            else if (player.HandValue() > Player2.HandValue() && player.HandValue() <= 21 && Player2.HandValue() <= 21 || player.HandValue() < Player2.HandValue() && player.HandValue() <= 21 && Player2.HandValue() > 21)
            {
                WinnerText.text = "Player 1 Wins!";
                int.TryParse(CurrentBet.text, out y);
                int.TryParse(Player1Liqudity.text, out z);
                z += y;
                Player1Liqudity.text = z.ToString();
                CurrentBet.text = "0";
                Player1Con++;
                Player2Con--;
                DealerCon--;
            }
            else if (Player2.HandValue() >= player.HandValue() && Player2.HandValue() <= 21 && player.HandValue() <= 21 || Player2.HandValue() < player.HandValue() && Player2.HandValue() <= 21 && player.HandValue() > 21)
            {
                WinnerText.text = "Player 2 Wins!";
                int.TryParse(CurrentBet.text, out y);
                int.TryParse(Player2Cash.text, out x);
                x += y;
                Player2Cash.text = x.ToString();
                CurrentBet.text = "0";
                Player2Con++;
                Player1Con--;
                DealerCon--;
            }
            else
            {
                WinnerText.text = "Dealer Wins";
                CurrentBet.text = "0";
                DealerCon++;
                Player1Con--;
                Player2Con--;
            }
        }
        PlayAgainButton.interactable = true;
    }
    IEnumerator AITurn()
    {

        if (Player2.HandValue() > 20)
        {
            StopAllCoroutines();
            StartCoroutine(DealersTurn());
           
        }
        yield return new WaitForSeconds(1f);
        double L = GameLogic();
        L *= 10;
        double C = Player2Con;
        double G = Greed();
        double R = Random.value;
        double T = 0.35;
        double Q = 0;
         Q =( (L * 0.4) + (0.1 * C) + (0.4 * G) + R)/10;

        Debug.Log(Q);
        if (Q >= T)
        {
            Player2.Push(deck.Pop());
            StartCoroutine(AITurn());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(DealersTurn());
            yield return new WaitForSeconds(30f);
        }

    }
    int GameLogic()
    {
        int y = 0;
        int x = 0;

        hitButton.interactable = false;
        stickButton.interactable = false;
         x = Player2.HandValue();
        y = dealer.HandValue();

        if (x <= 4 && x >= 11)
        {

            return 1;
        }
        else if (x > 11 && y >= 7)
        {
            
            return 1;
        }
        else if (x >= 12 && x <= 16 && y >= 7 && y <= 11)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
    void AIBet()
    {
        int StartingBet = 0;
        int Playercash = 0;
        int PlayerBet = 0;
        int.TryParse(Player2Cash.text, out StartingBet);
        int temp = StartingBet;
        StartingBet = StartingBet / 5;

        int.TryParse(Player1Liqudity.text, out Playercash);
        int.TryParse(CurrentBet.text, out PlayerBet);
        PlayerBet = PlayerBet / 2;

        int BettingDifference = PlayerBet - StartingBet;


        if (BettingDifference == 0)
        {
            StartingBet += 5;
        }
        else if (BettingDifference > StartingBet)
        {
            StartingBet *= 2;
            if (StartingBet > temp)
            {
                StartingBet -= BettingDifference;

            }
        }
        else if ((StartingBet * 2) < PlayerBet)
            StartingBet *= 2;



        PlayerBet *= 2;
        temp -= StartingBet;
        Player2Cash.text = temp.ToString();

        PlayerBet += (StartingBet * 2);

        CurrentBet.text = PlayerBet.ToString();


    }

    double Greed()
    {
        double x = 0;
        double y = 0;
        double.TryParse(CurrentBet.text, out y);
        double.TryParse(Player2Cash.text, out x);
        //400,000 is the highest possible aamount of money in the game as such it's our 100% value
        x = x / 4000;   y = y / 4000;

        if (x > y * 2)
        {
            return 10;
        }
        if (x < 10)
            return 0;
        else if (x > 10 && x < 20)
            return 1;
        else if (x > 20 && x < 30)
            return 2;
        else if (x > 30 && x < 40)
            return 3;
        else if (x > 40 && x < 50)
            return 4;
        else if (x > 50 && x < 60)
            return 5;
        else if (x > 60 && x < 70)
            return 6;
        else if (x > 70 && x < 80)
            return 7;
        else if (x > 80 && x < 90)
            return 8;
        else if (x > 90 && x < 99)
            return 9;
        else
            return 10;
    }
}
