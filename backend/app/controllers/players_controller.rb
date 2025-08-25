class PlayersController < ApplicationController
  before_action :set_player, only: %i[ show update destroy ]
  # GET /players
  def index
    @players = Player.all

    render json: @players
  end

  # GET /players/1
  def show
    render json: @player
  end

  def login
    player = Player.find_by(username: params[:username])
    if player && player.authenticate(params[:password])
      render json: { message: "Login successful", player: player }
    else
      render json: { error: "Invalid credentials", param: params }, status: :unauthorized
    end
  end

  def register
    player = Player.new(username: params[:username], password: params[:password])
    if player.save
      render json: { message: "Registration successful", player: player }
    else
      render json: { errors: player.errors.full_messages }, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /players/1
  def update
    if @player.update(player_params)
      render json: @player
    else
      render json: @player.errors, status: :unprocessable_entity
    end
  end

  # DELETE /players/1
  def destroy
    @player.destroy!
  end


  def ranking
    @player = Player.find(params[:id])  # Obtener el jugador por su ID

    # Calcular el puntaje total del jugador
    total_score = @player.matches.sum(:score)

    # Obtener el mejor puntaje y la fecha de ese match
    best_match = @player.matches.order(score: :desc).first
    best_score = best_match&.score || 0
    best_score_date = best_match&.created_at

    # Obtener los partidos (matches) del jugador
    player_matches = @player.matches.select(:id, :score, :created_at, :updated_at, :level, :time, :recolected)

    # Obtener el ranking global de los 5 mejores jugadores, incluyendo al jugador actual
    @ranking = Player.joins("LEFT JOIN matches ON matches.player_id = players.id")
                     .select("players.id, players.username, COALESCE(SUM(matches.score), 0) AS total_score")
                     .group("players.id")
                     .order("total_score DESC")
                     .limit(5)

    # Devolvemos el JSON con el jugador, su puntaje y los matches
    render json: {
      player: {
        id: @player.id,
        username: @player.username,
        total_score: total_score,
        best_score: best_score,
        best_score_date: best_score_date
      },
      ranking: {
        player_matches: player_matches.map { |match|
          {
            id: match.id,
            score: match.score,
            created_at: match.created_at,
            updated_at: match.updated_at,
            level: match.level,
            time: match.time,
            recolected: match.recolected
          }
        },
        global_ranking: @ranking.map { |player|
          best_match = player.matches.order(score: :desc, created_at: :asc).first
          {
            id: player.id,
            username: player.username,
            total_score: player.total_score,
            best_score: best_match&.score || 0,
            best_score_date: best_match&.created_at
          }
        }
      }
    }
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_player
      @player = Player.find(params.expect(:id))
    end

    # Only allow a list of trusted parameters through.
    def player_params
      params.expect(:username, :password, :password_confirmation)
    end
end
