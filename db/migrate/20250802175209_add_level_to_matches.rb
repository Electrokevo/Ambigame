class AddLevelToMatches < ActiveRecord::Migration[8.0]
  def change
    add_column :matches, :level, :integer
  end
end
