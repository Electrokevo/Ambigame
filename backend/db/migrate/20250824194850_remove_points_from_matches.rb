class RemovePointsFromMatches < ActiveRecord::Migration[8.0]
  def change
    remove_column :matches, :points, :integer
  end
end
