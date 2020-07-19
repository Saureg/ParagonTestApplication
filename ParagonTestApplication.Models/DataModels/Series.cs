namespace ParagonTestApplication.Models.DataModels
{
    /// <summary>
    /// Series model
    /// </summary>
    public class Series
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether idDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}