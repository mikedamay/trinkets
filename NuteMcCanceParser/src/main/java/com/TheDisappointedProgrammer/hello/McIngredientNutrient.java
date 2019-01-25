package com.TheDisappointedProgrammer.hello;

public class McIngredientNutrient {
    private String ingredientCode;
    private String nutrientCode;
    private String value;

    public McIngredientNutrient(String ingredientCode, String nutrientCode, String value) {
        this.ingredientCode = ingredientCode;
        this.nutrientCode = nutrientCode;
        this.value = value;
    }

    public String getIngredientCode() {
        return ingredientCode;
    }

    public String getNutrientCode() {
        return nutrientCode;
    }

    public String getValue() {
        return value;
    }
}
