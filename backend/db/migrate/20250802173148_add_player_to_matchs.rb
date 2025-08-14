class AddPlayerToMatchs < ActiveRecord::Migration[8.0]
  def change
    add_reference :matches, :player, null: false, foreign_key: true
  end
end
