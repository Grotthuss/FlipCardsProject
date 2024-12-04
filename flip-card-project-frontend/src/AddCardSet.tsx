import React, { useState } from 'react';
import './AddCardSet.css';

interface AddCardSetProps {
    onAdd: (id: number, setName: string) => void;
}

const AddCardSet: React.FC<AddCardSetProps> = ({ onAdd }) => {
    const [setName, setSetName] = useState('');
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        const requestData = {
            userId: 1,
            name: setName.trim(),
            flipcardsList: [],
        };

        try {
            const validationResponse = await fetch(`https://localhost:44372/api/Home/ValidateSet`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(requestData),
            });

            if (!validationResponse.ok) {
                const errorMessage = await validationResponse.text();
                setError(`Validation failed: ${errorMessage}`);
                return;
            }

            const response = await fetch(`https://localhost:44372/api/Home/CreateFullSet`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(requestData),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(`Failed to create set. Server response: ${errorMessage}`);
            }

            const newCardSet = await response.json();

            if (newCardSet && newCardSet.id && newCardSet.name) {
                onAdd(newCardSet.id, newCardSet.name);
                setSetName('');
            } else {
                throw new Error("Unexpected response format from CreateFullSet API.");
            }
        } catch (err) {
            setError((err as Error).message);
        }
    };

    return (
        <form className="add-card-set-container" onSubmit={handleSubmit}>
            <h2>Add New Card Set</h2>
            <input
                type="text"
                placeholder="Enter card set name"
                value={setName}
                onChange={(e) => setSetName(e.target.value)}
                required
            />
            {error && <p className="error-message">{error}</p>}
            <button type="submit">Add Card Set</button>
        </form>
    );
};

export default AddCardSet;
