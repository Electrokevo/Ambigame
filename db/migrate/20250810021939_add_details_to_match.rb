class AddDetailsToMatch < ActiveRecord::Migration[8.0]
  def change
    add_column :matches, :time, :integer
    add_column :matches, :points, :integer
    add_column :matches, :recolected, :integer
  end
end
