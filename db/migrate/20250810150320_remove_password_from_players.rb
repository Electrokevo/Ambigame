class RemovePasswordFromPlayers < ActiveRecord::Migration[8.0]
  def change
    remove_column :players, :password, :string
  end
end
