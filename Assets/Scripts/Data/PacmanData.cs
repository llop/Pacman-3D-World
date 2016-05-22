public class PacmanData {


  private bool _powerMode;
  private int _lives;
  private int _score;


  public PacmanData(bool powerMode, int lives, int score) {
    _powerMode = powerMode;
    _lives = lives;
    _score = score;
  }


  public bool powerMode {
    get { return _powerMode; }
    set { _powerMode = value; }
  }


  public int score() { return _score; }
  public void addScore(int score) {
    _score += score;
  }


  public int lives() { return _lives; }
  public void addLives(int lives) {
    _lives += lives;
  }


}
