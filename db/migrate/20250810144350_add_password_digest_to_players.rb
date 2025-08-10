class AddPasswordDigestToPlayers < ActiveRecord::Migration[8.0]
  def change
    add_column :players, :password_digest, :string
  end
end
